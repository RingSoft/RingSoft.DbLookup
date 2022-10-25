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
    ///     <MyNamespace:RecordLockingWindow/>
    ///
    /// </summary>
    public class RecordLockingWindow : BaseWindow, IRecordLockingView
    {
        public StackPanel ButtonsPanel { get; set; }
        public Border Border { get; set; }
        public RecordLockingViewModel ViewModel { get; set; }
        public IDbMaintenanceProcessor Processor { get; set; }

        private Control _buttonsControl;
        private LookupAddViewArgs _addViewArgs;

        static RecordLockingWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RecordLockingWindow), new FrameworkPropertyMetadata(typeof(RecordLockingWindow)));
        }

        public void Initialize()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            _buttonsControl = LookupControlsGlobals.DbMaintenanceButtonsFactory.GetRecordLockingButtonsControl(ViewModel);
            if (ButtonsPanel != null)
            {
                ButtonsPanel.Children.Add(_buttonsControl);
                ButtonsPanel.UpdateLayout();
            }
            ViewModel.View = this;
            Processor.Initialize(this, _buttonsControl, ViewModel, this);
            if (_addViewArgs != null)
            {
                Processor.InitializeFromLookupData(_addViewArgs);
            }

        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ButtonsPanel = GetTemplateChild(nameof(ButtonsPanel)) as StackPanel;
            ViewModel = Border.TryFindResource("RecordLockingViewModel") as RecordLockingViewModel;

            Initialize();

            base.OnApplyTemplate();
        }

        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        public void ResetViewForNewRecord()
        {
            
        }

        public void SetupView()
        {
            
        }
    }
}
