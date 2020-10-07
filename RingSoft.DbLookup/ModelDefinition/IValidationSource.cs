using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.ModelDefinition
{
    public interface IValidationSource
    {
        AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition);

        void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption);

        bool ValidateEntityProperty(FieldDefinition fieldDefinition, string valueToValidate);
    }
}
