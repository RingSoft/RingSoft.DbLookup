using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;
using System.Windows;
using System.Windows.Controls;

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
    public class DeleteRecordWindow : BaseWindow
    {
        public TabControl TabControl { get; private set; }
        public Border Border { get; set; }
        public CheckBox CheckBox { get; set; }

        public DeleteTables DeleteTables { get; private set; }

        static DeleteRecordWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeleteRecordWindow), new FrameworkPropertyMetadata(typeof(DeleteRecordWindow)));
        }

        public DeleteRecordWindow(DeleteTables deleteTables)
        {
            DeleteTables = deleteTables;
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            CheckBox = GetTemplateChild(nameof(CheckBox)) as CheckBox;
            //TabControl = GetTemplateChild(nameof(TabControl)) as TabControl;

            //foreach (var deleteTable in DeleteTables.Tables)
            //{
            //    var tabItem = new TabItem();
            //    tabItem.Header = deleteTable.ChildField.TableDefinition.Description;
            //    tabItem.Content = new DeleteRecordWindowItemControl(deleteTable);
            //    tabItem.Focusable = true;
            //    TabControl.Items.Add(tabItem);
            //}

            Border.GotFocus += (sender, args) =>
            {
                //TabControl.Focus();
                //TabControl.SelectedIndex = 0;
                CheckBox.Focus();
            };

            base.OnApplyTemplate();
        }
    }
}
