﻿using System.Linq;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.Lookup;
using TreeViewItem = RingSoft.DbMaintenance.TreeViewItem;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.AdvancedFind"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.AdvancedFind;assembly=RingSoft.DbLookup.Controls.WPF.AdvancedFind"
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
    ///     <MyNamespace:AdvancedFindWindow/>
    ///
    /// </summary>
    [TemplatePart(Name = "ButtonsPanel", Type = typeof(StackPanel))]
    public class AdvancedFindWindow : BaseWindow, IAdvancedFindView
    {
        public StackPanel ButtonsPanel { get; set; }
        public Border Border { get; set; }
        public AutoFillControl NameAutoFillControl { get; set; }
        public TextComboBoxControl TableComboBoxControl { get; set; }
        public LookupControl LookupControl { get; set; }

        public AdvancedFindViewModel ViewModel { get; set; }

        private Control _buttonsControl;
        private LookupAddViewArgs _addViewArgs;

        public IDbMaintenanceProcessor Processor { get; set; }

        static AdvancedFindWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindWindow)));
        }

        public AdvancedFindWindow(LookupAddViewArgs addViewArgs)
        {
            _buttonsControl = LookupControlsGlobals.DbMaintenanceButtonsFactory.GetButtonsControl();
            _addViewArgs = addViewArgs;

        }

        public AdvancedFindWindow()
        {
            _buttonsControl = LookupControlsGlobals.DbMaintenanceButtonsFactory.GetButtonsControl();
        }

        public void Initialize()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            ViewModel = Border.TryFindResource("AdvancedFindViewModel") as AdvancedFindViewModel;
            ViewModel.View = this;
            Processor.Initialize(this, _buttonsControl, ViewModel, this);
            if (_addViewArgs != null)
            {
                Processor.InitializeFromLookupData(_addViewArgs);
                if (_addViewArgs.InputParameter is AdvancedFindInput advancedFindInput)
                {
                    TableComboBoxControl.IsEnabled = false;
                    if (!advancedFindInput.LookupWidth.Equals(0))
                    {
                        LookupControl.MinWidth = 0;
                        LookupControl.Width = advancedFindInput.LookupWidth;
                        LookupControl.HorizontalAlignment = HorizontalAlignment.Left;
                    }
                }
            }
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ButtonsPanel = GetTemplateChild(nameof(ButtonsPanel)) as StackPanel;
            NameAutoFillControl = GetTemplateChild(nameof(NameAutoFillControl)) as AutoFillControl;
            TableComboBoxControl = GetTemplateChild(nameof(TableComboBoxControl)) as TextComboBoxControl;
            LookupControl = GetTemplateChild(nameof(LookupControl)) as LookupControl;
            
            if (ButtonsPanel != null)
            {
                ButtonsPanel.Children.Add(_buttonsControl);
                ButtonsPanel.UpdateLayout();
            }

            if (LookupControl != null)
            {
                LookupControl.ColumnWidthChanged += (sender, args) =>
                {
                    if (ViewModel.LookupDefinition.VisibleColumns.Contains(args.ColumnDefinition))
                    {
                        ViewModel.ColumnsManager.UpdateColumnWidth(args.ColumnDefinition);
                    }
                };
            }

            Initialize();
            if (NameAutoFillControl != null)
            {
                Processor.RegisterFormKeyControl(NameAutoFillControl);
            }

            base.OnApplyTemplate();
        }

        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Processor.OnValidationFail(fieldDefinition, text, caption);
        }

        public void ResetViewForNewRecord()
        {
            NameAutoFillControl?.Focus();
        }

        public bool ShowFormulaEditor(TreeViewItem formulaTreeViewItem)
        {
            var editor = new AdvancedFindFormulaColumnWindow(new DataEntryGridMemoValue(0));
            if (formulaTreeViewItem.Parent != null)
            {
                editor.ParentTable = formulaTreeViewItem.Parent.FieldDefinition.ParentJoinForeignKeyDefinition
                    .ForeignTable.Description;
                editor.ParentField = formulaTreeViewItem.Parent.FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0]
                    .ForeignField.Description;
            }
            else
            {
                editor.ParentTable = formulaTreeViewItem.ViewModel.LookupDefinition.TableDefinition.Description;
                editor.ParentField = "<Lookup Root>";
            }

            if (formulaTreeViewItem.FormulaData != null)
            {
                editor.DataType = formulaTreeViewItem.FormulaData.DataType;
            }
            editor.Owner =this;
            editor.ShowInTaskbar = false;
            if (editor.ShowDialog())
            {
                formulaTreeViewItem.FormulaData = new TreeViewFormulaData();
                formulaTreeViewItem.FormulaData.DataType = editor.ViewModel.DataType;
                formulaTreeViewItem.FormulaData.Formula = editor.MemoEditor.Text;
                return true;
            }
            return false;
        }
    }
}
