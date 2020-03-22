using System;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// An AutoFill field.
    /// </summary>
    /// <seealso cref="AutoFillBase" />
    public class AutoFillFieldDefinition : AutoFillBase
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override AutoFillTypes Type => AutoFillTypes.Field;

        /// <summary>
        /// Gets the string field definition.
        /// </summary>
        /// <value>
        /// The string field definition.
        /// </value>
        public StringFieldDefinition StringFieldDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether this auto fill is distinct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if distinct; otherwise, <c>false</c>.
        /// </value>
        public bool Distinct { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillFieldDefinition"/> class.
        /// </summary>
        /// <param name="stringFieldDefinition">The string field definition.</param>
        public AutoFillFieldDefinition(StringFieldDefinition stringFieldDefinition) 
            : base(stringFieldDefinition.TableDefinition)
        {
            StringFieldDefinition = stringFieldDefinition;
        }

        /// <summary>
        /// Determines whether this auto fill is distinct.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>This object for fluent processing.</returns>
        public AutoFillFieldDefinition IsDistinct(bool value = true)
        {
            var isPrimaryKey = StringFieldDefinition.TableDefinition.PrimaryKeyFields.Count > 1 &&
                               StringFieldDefinition.TableDefinition.PrimaryKeyFields.Contains(StringFieldDefinition);
            if (!isPrimaryKey && value)
                throw new ArgumentException(
                    "The distinct value can only be set on primary key field auto fills where there are at least 2 fields in the primary key.");

            Distinct = value;
            return this;
        }

    }
}
