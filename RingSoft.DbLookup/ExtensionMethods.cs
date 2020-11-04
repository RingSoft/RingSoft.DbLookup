using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
    public static class ExtensionMethods
    {

        public static string TrimRight(this string value, string trimChars)
        {
            return value.LeftStr(value.Length - trimChars.Length);
        }

        #region Property Name

        /// <summary>
        /// Gets the full name of the property. (u => u.UserId returns "UserId")
        /// </summary>
        /// <typeparam name="T">The first parameter of the Func.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="exp">The expression.</param>
        /// <returns>The full name of the property.</returns>
        public static string GetFullPropertyName<T, TProperty>
            (this Expression<Func<T, TProperty>> exp)
        {
            MemberExpression memberExp;
            if (!TryFindMemberExpression(exp.Body, out memberExp))
                return string.Empty;

            var memberNames = new Stack<string>();
            do
            {
                memberNames.Push(memberExp.Member.Name);
            } while (TryFindMemberExpression(memberExp.Expression, out memberExp));

            return string.Join(".", memberNames.ToArray());
        }

        // code adjusted to prevent horizontal overflow
        private static bool TryFindMemberExpression
            (Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;
            if (memberExp != null)
            {
                // heyo! that was easy enough
                return true;
            }

            // if the compiler created an automatic conversion,
            // it'll look something like...
            // obj => Convert(obj.Property) [e.g., int -> object]
            // OR:
            // obj => ConvertChecked(obj.Property) [e.g., int -> long]
            // ...which are the cases checked in IsConversion
            if (IsConversion(exp) && exp is UnaryExpression)
            {
                memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
                if (memberExp != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConversion(Expression exp)
        {
            return (
                exp.NodeType == ExpressionType.Convert ||
                exp.NodeType == ExpressionType.ConvertChecked
                );
        }

        #endregion

        public static LookupColumnSearchForControls TranslateFieldDataTypeToControl(this FieldDataTypes dataType)
        {
            switch (dataType)
            {
                case FieldDataTypes.String:
                case FieldDataTypes.Enum:
                case FieldDataTypes.Bool:
                    return LookupColumnSearchForControls.StringControl;
                case FieldDataTypes.Integer:
                    return LookupColumnSearchForControls.IntegerControl;
                case FieldDataTypes.Decimal:
                    return LookupColumnSearchForControls.DecimalControl;
                case FieldDataTypes.DateTime:
                    return LookupColumnSearchForControls.DateControl;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }
    }
}
