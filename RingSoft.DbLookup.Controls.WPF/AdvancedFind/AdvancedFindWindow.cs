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
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

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
    public class AdvancedFindWindow : BaseWindow, IDbMaintenanceView
    {
        public StackPanel ButtonsPanel { get; set; }
        public Border Border { get; set; }
        public AutoFillControl NameAutoFillControl { get; set; }

        public IDbMaintenanceProcessor Processor { get; set; }

        static AdvancedFindWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindWindow)));
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ButtonsPanel = GetTemplateChild(nameof(ButtonsPanel)) as StackPanel;
            NameAutoFillControl = GetTemplateChild(nameof(NameAutoFillControl)) as AutoFillControl;
            
            var advancedFindViewModel = Border.TryFindResource("AdvancedFindViewModel") as AdvancedFindViewModel;

            var buttonsControl = LookupControlsGlobals.DbMaintenanceButtonsFactory.GetButtonsControl();
            if (ButtonsPanel != null)
            {
                ButtonsPanel.Children.Add(buttonsControl);
                ButtonsPanel.UpdateLayout();
            }

            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            Processor.Initialize(this, buttonsControl, advancedFindViewModel, this);

            Processor.RegisterFormKeyControl(NameAutoFillControl);

            base.OnApplyTemplate();
        }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Processor.OnValidationFail(fieldDefinition, text, caption);
        }

        public void ResetViewForNewRecord()
        {
            
        }

        public void OnRecordSelected()
        {
            Processor.OnRecordSelected();
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            Processor.ShowFindLookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor, initialSearchForPrimaryKey);
        }

        public void CloseWindow()
        {
            Processor.CloseWindow();
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            return Processor.ShowYesNoCancelMessage(text, caption, playSound);
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            return Processor.ShowYesNoMessage(text, caption, playSound);
        }

        public void ShowRecordSavedMessage()
        {
            Processor.ShowRecordSavedMessage();
        }
    }
}
