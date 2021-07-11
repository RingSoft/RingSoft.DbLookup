using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Linq;
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

        private int _designerSelectedId;

        public int DesignerSelectedId
        {
            get => _designerSelectedId;
            set
            {
                if (_designerSelectedId == value)
                    return;

                _designerSelectedId = value;
                OnPropertyChanged();
            }
        }

        protected override void ProcessFrameworkElementFactory(LookupControl lookupControl,
            FrameworkElementFactory factory, string dataColumnName,
            LookupColumnDefinitionBase lookupColumnDefinition, bool designMode)
        {
            if (ContentTemplate == null)
                throw new Exception($"The {nameof(ContentTemplate)} Property has not been set.");

            factory.SetValue(CustomContentControl.ContentTemplateProperty, ContentTemplate);

            if (lookupColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                && lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerField)
            {
                if (integerField.EnumTranslation != null)
                {
                    factory.SetValue(LookupCustomContentControl.EnumFieldTranslationProperty, integerField.EnumTranslation);
                }
            }
            if (designMode)
            {
                factory.SetValue(LookupCustomContentControl.DesignerValueProperty, DesignerSelectedId);
            }
            else
            {
                factory.SetBinding(LookupCustomContentControl.DataTextProperty, new Binding(dataColumnName));
            }
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
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

        public static readonly DependencyProperty DataTextProperty =
            DependencyProperty.Register(nameof(DataText), typeof(string), typeof(LookupCustomContentControl),
                new FrameworkPropertyMetadata(DataTextChangedCallback));

        public string DataText
        {
            get { return (string)GetValue(DataTextProperty); }
            set { SetValue(DataTextProperty, value); }
        }

        private static void DataTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customControl = (LookupCustomContentControl)obj;
            customControl.SetDataText();
        }

        public static readonly DependencyProperty DesignerValueProperty =
            DependencyProperty.Register(nameof(DesignerValue), typeof(int), typeof(LookupCustomContentControl),
                new FrameworkPropertyMetadata(DesignerValueChangedCallback));

        public int DesignerValue
        {
            get { return (int)GetValue(DesignerValueProperty); }
            set { SetValue(DesignerValueProperty, value); }
        }

        private static void DesignerValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customControl = (LookupCustomContentControl)obj;
            customControl.SelectedItemId = customControl.DesignerValue;
        }

        static LookupCustomContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LookupCustomContentControl), new FrameworkPropertyMetadata(typeof(LookupCustomContentControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (!DataText.IsNullOrEmpty())
                SetDataText();
        }

        protected virtual void SetDataText()
        {
            if (EnumFieldTranslation == null)
            {
                SelectItem(DataText.ToInt());
            }
            else
            {
                var typeTranslation =
                    EnumFieldTranslation.TypeTranslations.FirstOrDefault(f => f.TextValue == DataText);
                if (typeTranslation != null)
                    SelectItem(typeTranslation.NumericValue);
            }
        }
    }
}
