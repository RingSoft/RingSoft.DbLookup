using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DbMaintenance;

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
    ///     <MyNamespace:DeleteWindowTabItem/>
    ///
    /// </summary>
    public class DeleteRecordWindowItemControl : Control
    {
        public Border Border { get; private set; }
        public CheckBox DeleteAllCheckBox { get; private set; }
        public CheckBox NullAllCheckBox { get; private set; }

        public DeleteRecordItemViewModel ViewModel { get; private set; }
        public DeleteTable DeleteTable { get; private set; }
        
        static DeleteRecordWindowItemControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeleteRecordWindowItemControl), new FrameworkPropertyMetadata(typeof(DeleteRecordWindowItemControl)));

            IsTabStopProperty.OverrideMetadata(typeof(DeleteRecordWindowItemControl), new FrameworkPropertyMetadata(false));

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DeleteRecordWindowItemControl),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
        }

        public DeleteRecordWindowItemControl(DeleteTable deleteTable)
        {
            DeleteTable = deleteTable;
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            DeleteAllCheckBox = GetTemplateChild(nameof(DeleteAllCheckBox)) as CheckBox;
            NullAllCheckBox = GetTemplateChild(nameof(NullAllCheckBox)) as CheckBox;
            
            ViewModel = Border.TryFindResource("ViewModel") as DeleteRecordItemViewModel;

            ViewModel.Initialize(DeleteTable);
            if (DeleteTable.ChildField.AllowNulls)
            {
                DeleteAllCheckBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                NullAllCheckBox.Visibility = Visibility.Collapsed;
            }
            base.OnApplyTemplate();
        }
    }
}
