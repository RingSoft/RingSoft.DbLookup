using System;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupControlColumnFactory
    {
        public virtual LookupColumnBase CreateLookupColumn(LookupColumnDefinitionBase columnDefinition)
        {
            if (columnDefinition.LookupControlColumnId == LookupDefaults.CustomContentColumnId)
            {
                
                var customContentColumn = new LookupCustomContentColumn();
                if (columnDefinition.ContentTemplateId == null)
                {
                    if (LookupControlsGlobals.LookupControlContentTemplateFactory == null)
                        throw new Exception(
                            $"{nameof(ILookupControlContentTemplateFactory)} must be implemented and set to the {LookupControlsGlobals.LookupControlContentTemplateFactory} static property in order to use CustomContent.");

                    var contentTemplateId = columnDefinition.ContentTemplateId;
                    if (contentTemplateId == null && columnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                                                  && lookupFieldColumn.FieldDefinition is IntegerFieldDefinition
                                                      integerFieldDefinition)
                        contentTemplateId = integerFieldDefinition.ContentTemplateId;

                    if (contentTemplateId == null)
                        throw new Exception(
                            $"No ContentTemplateId found for LookupColumnDefinition {columnDefinition.Caption}");

                    customContentColumn.ContentTemplate =
                        LookupControlsGlobals.LookupControlContentTemplateFactory.GetContentTemplate(contentTemplateId
                            .Value);

                    if (customContentColumn.ContentTemplate == null)
                        throw new Exception(
                            $"No ContentTemplate found for Id {contentTemplateId.Value}, Column: {columnDefinition.Caption}");
                }
                return customContentColumn;
            }

            var result = new LookupColumn{TextAlignment = columnDefinition.HorizontalAlignment};
            return result;
        }
    }
}
