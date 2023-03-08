using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;
using System.Windows;
using System.Windows.Controls;
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
    ///     <MyNamespace:DeleteRecordWindow/>
    ///
    /// </summary>
    public class DeleteRecordWindow : BaseWindow, IDeleteRecordView
    {
        //public TabControl TabControl { get; private set; }
        public Border Border { get; set; }
        public DeleteRecordViewModel ViewModel { get; private set; }
        public CheckBox DeleteAllCheckBox { get; private set; }
        public LookupControl LookupControl { get; private set; }

        private Size _oldSize;
        public List<DeleteRecordWindowItemControl> DeleteTabs { get; private set; } =
            new List<DeleteRecordWindowItemControl>();


        static DeleteRecordWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeleteRecordWindow), new FrameworkPropertyMetadata(typeof(DeleteRecordWindow)));
        }

        public DeleteRecordWindow(DeleteTables deleteTables)
        {
            var loaded = false;
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, deleteTables);

                DeleteAllCheckBox.Focus();
                loaded = true;
            };
            SizeChanged += (sender, args) =>
            {
                if (LookupControl != null && loaded)
                {
                    var widthDif = Width - _oldSize.Width;
                    var heightDif = Height - _oldSize.Height;
                    if (Math.Round(widthDif) > 1)
                    {
                        LookupControl.Width = LookupControl.ActualWidth + widthDif;
                    }

                    if (Math.Round(heightDif) > 1)
                    {
                        LookupControl.Height = LookupControl.ActualHeight + heightDif;
                    }
                }

                _oldSize = args.NewSize;
            };

        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            DeleteAllCheckBox = GetTemplateChild(nameof(DeleteAllCheckBox)) as CheckBox;
            LookupControl = GetTemplateChild(nameof(LookupControl)) as LookupControl;
            ViewModel = Border.TryFindResource("ViewModel") as DeleteRecordViewModel;

            base.OnApplyTemplate();
        }

        public void CloseWindow(bool result)
        {
            DialogResult = result;
            Close();
        }
    }
}
