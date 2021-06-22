using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System.Windows;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupCustomContentColumn : LookupColumn<LookupCustomContentControl>
    {
        private DataEntryCustomContentTemplate _contentTemplate;

        public DataEntryCustomContentTemplate ContentTemplate
        {
            get => _contentTemplate;
            set
            {
                if (_contentTemplate == value)
                    return;

                _contentTemplate = value;
                OnPropertyChanged(nameof(ContentTemplate));
            }
        }

        protected override void ProcessFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName,
            LookupColumnDefinitionBase lookupColumnDefinition)
        {
            if (lookupColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                && lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerField
                && integerField.EnumTranslation != null)
            {
                factory.SetValue(LookupCustomContentControl.EnumFieldTranslationProperty, integerField.EnumTranslation);
            }
            factory.SetValue(CustomContentControl.ContentTemplateProperty, ContentTemplate);
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            factory.SetBinding(LookupCustomContentControl.EnumTextProperty, new Binding(dataColumnName));
        }
    }

    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.LookupControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.LookupControl;assembly=RingSoft.DbLookup.Controls.WPF.LookupControl"
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
    ///     <MyNamespace:CustomContentLookupControl/>
    ///
    /// </summary>
    public class LookupCustomContentControl : CustomContentControl
    {
        public static readonly DependencyProperty EnumFieldTranslationProperty =
            DependencyProperty.Register(nameof(EnumFieldTranslation), typeof(EnumFieldTranslation), typeof(LookupCustomContentControl));

        public EnumFieldTranslation EnumFieldTranslation
        {
            get { return (EnumFieldTranslation)GetValue(EnumFieldTranslationProperty); }
            set { SetValue(EnumFieldTranslationProperty, value); }
        }

        public static readonly DependencyProperty EnumTextProperty =
            DependencyProperty.Register(nameof(EnumText), typeof(string), typeof(LookupCustomContentControl),
                new FrameworkPropertyMetadata(EnumTextChangedCallback));

        public string EnumText
        {
            get { return (string)GetValue(EnumTextProperty); }
            set { SetValue(EnumTextProperty, value); }
        }

        private static void EnumTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customControl = (LookupCustomContentControl)obj;
            customControl.SetEnumText();
        }

        static LookupCustomContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LookupCustomContentControl), new FrameworkPropertyMetadata(typeof(LookupCustomContentControl)));
        }

        private void SetEnumText()
        {

        }
    }
}
