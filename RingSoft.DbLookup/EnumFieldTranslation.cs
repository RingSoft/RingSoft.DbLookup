using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// A list of enum numeric values and their corresponding description values.
    /// </summary>
    public class EnumFieldTranslation
    {
        private readonly List<TypeTranslation> _typeTranslations = new List<TypeTranslation>();

        /// <summary>
        /// Gets the type translations.
        /// </summary>
        /// <value>
        /// The type translations.
        /// </value>
        public IReadOnlyList<TypeTranslation> TypeTranslations => _typeTranslations;

        internal EnumFieldTranslation()
        {
            
        }

        internal void LoadFromEnum<T>() where T : Enum
        {
            LoadFromEnum(typeof(T));
        }

        internal void LoadFromEnum(Type enumType)
        {
            var enumValues = Enum.GetValues(enumType);

            foreach (var enumValue in enumValues)
            {
                var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var textValue = attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();

                var typeTranslation = new TypeTranslation
                {
                    NumericValue = (int)enumValue,
                    TextValue = textValue
                };
                _typeTranslations.Add(typeTranslation);
            }
        }

        internal void LoadFromBoolean(string trueText, string falseText)
        {
            _typeTranslations.Add(new TypeTranslation
            {
                NumericValue = 0,
                TextValue = falseText
            });

            _typeTranslations.Add(new TypeTranslation
            {
                NumericValue = 1,
                TextValue = trueText
            });
        }

        /// <summary>
        /// A database field's numeric value and its corresponding text value.
        /// </summary>
        public class TypeTranslation
        {
            /// <summary>
            /// Gets the numeric value.
            /// </summary>
            /// <value>
            /// The numeric value.
            /// </value>
            public int NumericValue { get; internal set; }

            /// <summary>
            /// Gets the text value.
            /// </summary>
            /// <value>
            /// The text value.
            /// </value>
            public string TextValue { get; internal set; }

            public override string ToString()
            {
                return TextValue;
            }
        }
    }
}
