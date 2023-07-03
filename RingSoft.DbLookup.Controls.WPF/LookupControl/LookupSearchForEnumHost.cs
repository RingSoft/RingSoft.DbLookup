using RingSoft.DataEntryControls.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Controls.WPF
{
    internal class LookupSearchForEnumHost : LookupSearchForHost<TextComboBoxControl>
    {
        public override string SearchText
        {
            get => Control.SelectedItem.NumericValue.ToString();
            set
            {
                var numVal = value.ToInt();
                var item = Setup.Items.FirstOrDefault(
                    p => p.NumericValue == numVal);
                Control.SelectedItem = item;
            } 
        }
        public TextComboBoxControlSetup Setup { get; set; }

        public double DefaultWidth { get; set; } = 100;

        public override void SelectAll()
        {
            
        }

        protected override TextComboBoxControl ConstructControl()
        {
            return new TextComboBoxControl();
        }

        protected override void Initialize(TextComboBoxControl control, LookupColumnDefinitionBase columnDefinition)
        {
            Setup = new TextComboBoxControlSetup();
            if (columnDefinition is LookupFieldColumnDefinition fieldColumn)
            {
                if (fieldColumn.FieldDefinition is IntegerFieldDefinition integerField)
                {
                    Setup.LoadFromEnum(integerField.EnumTranslation);
                }
            }

            control.SelectionChanged += (sender, args) =>
            {
                OnTextChanged();
            };
            control.Setup = Setup;
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.Width = DefaultWidth;
        }

        protected override void Initialize(TextComboBoxControl control)
        {
            
        }
    }
}
