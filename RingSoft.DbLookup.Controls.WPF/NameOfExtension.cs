using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace RingSoft.DbLookup.Controls.WPF
{
    [ContentProperty(nameof(Member))]
    public class NameOfExtension : MarkupExtension
    {
        public Type Type { get; set; }
        public string Member { get; private set; }

        public NameOfExtension(string member)
        {
            Member = member;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            if (Type == null || Member.Contains("."))
                throw new ArgumentException("Syntax for x:NameOf is [propertyName], Type={x:Type [className]}");

            if (string.IsNullOrEmpty(Member))
                return Member;

            var pinfo = Type.GetRuntimeProperties().FirstOrDefault(pi => pi.Name == Member);
            var finfo = Type.GetRuntimeFields().FirstOrDefault(fi => fi.Name == Member);
            if (pinfo == null && finfo == null)
                throw new ArgumentException($"No property or field found for {Member} in {Type}");

            return Member;
        }
    }
}
