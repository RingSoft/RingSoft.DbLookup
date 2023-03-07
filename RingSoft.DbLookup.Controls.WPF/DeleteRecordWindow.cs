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
        //public StackPanel SingleTabPanel { get; private set; }
        public TextBlock SingleTextBlock { get; private set; }
        public DeleteRecordViewModel ViewModel { get; private set; }
        public CheckBox DeleteAllCheckBox { get; set; }

        public DeleteTables DeleteTables { get; private set; }
        public List<DeleteRecordWindowItemControl> DeleteTabs { get; private set; } =
            new List<DeleteRecordWindowItemControl>();


        static DeleteRecordWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeleteRecordWindow), new FrameworkPropertyMetadata(typeof(DeleteRecordWindow)));
        }

        public DeleteRecordWindow(DeleteTables deleteTables)
        {
            DeleteTables = deleteTables;
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, deleteTables);
                //if (deleteTables.Tables.Count == 1)
                //{
                //    //TabControl.Visibility = Visibility.Collapsed;
                //    DeleteAllCheckBox.Visibility = Visibility.Collapsed;
                //    var deleteTable = deleteTables.Tables[0];
                //    var deleteTab = new DeleteRecordWindowItemControl(deleteTable);
                //    deleteTable.Description =
                //        $"{deleteTable.ChildField.TableDefinition.Description}\r\n{deleteTable.ChildField.Description}";
                //    SingleTextBlock.Text = deleteTable.Description;

                //    DeleteTabs.Add(deleteTab);
                //    SingleTabPanel.Children.Add(deleteTab);
                //    deleteTab.Loaded += (o, eventArgs) =>
                //    {
                //        deleteTab.SetInitialFocusCheckBox();
                //    };
                //}
                //else
                //{
                //    //SingleTabPanel.Visibility = Visibility.Collapsed;
                //    foreach (var deleteTable in DeleteTables.Tables)
                //    {
                //        //var tabItem = new TabItem();
                //        //deleteTable.Description =
                //        //    $"{deleteTable.ChildField.TableDefinition.Description}\r\n{deleteTable.ChildField.Description}";
                //        //var textBlockHeader = new TextBlock { Text = deleteTable.Description };
                //        //textBlockHeader.Margin = new Thickness(5);
                //        //tabItem.Header = textBlockHeader;
                //        //var deleteTab = new DeleteRecordWindowItemControl(deleteTable);
                //        //DeleteTabs.Add(deleteTab);
                //        //tabItem.Content = deleteTab;
                //        //var left = tabItem.Margin.Left;
                //        //var bottom = tabItem.Margin.Bottom;
                //        //var right = tabItem.Margin.Right;
                //        //tabItem.Margin = new Thickness(left, 2, right, bottom);
                //        //TabControl.Items.Add(tabItem);
                //    }
                //}

                DeleteAllCheckBox.Focus();
            };
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            DeleteAllCheckBox = GetTemplateChild(nameof(DeleteAllCheckBox)) as CheckBox;

            //TabControl = GetTemplateChild(nameof(TabControl)) as TabControl;
            //SingleTabPanel = GetTemplateChild(nameof(SingleTabPanel)) as StackPanel;
            SingleTextBlock = GetTemplateChild(nameof(SingleTextBlock)) as TextBlock;
            ViewModel = Border.TryFindResource("ViewModel") as DeleteRecordViewModel;

            base.OnApplyTemplate();
        }

        public void CloseWindow(bool result)
        {
            DialogResult = result;
            Close();
        }

        public void SetAllDataDelete(bool value)
        {
            foreach (var deleteTab in DeleteTabs)
            {
                if (deleteTab.ViewModel != null)
                {
                    deleteTab.ViewModel.DeleteAllRecords = value;
                    deleteTab.ViewModel.NullAllRecords = value;
                }
            }

        }

        public void SetAllDataNull(bool value)
        {
            foreach (var deleteTab in DeleteTabs)
            {
                if (deleteTab.ViewModel != null)
                {
                    deleteTab.ViewModel.NullAllRecords = value;
                }
            }
        }

        public void SetFocusToTable(DeleteTable deleteTable)
        {
            var deleteItem = DeleteTabs.FirstOrDefault(p => p.DeleteTable == deleteTable);
            if (deleteItem != null)
            {
                var tabItem = deleteItem.GetParentOfType<TabItem>();
                if (tabItem != null)
                {
                    tabItem.Focus();
                }
            }
        }
    }
}
