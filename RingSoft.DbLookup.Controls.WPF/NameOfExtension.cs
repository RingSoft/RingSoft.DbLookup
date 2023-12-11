// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="NameOfExtension.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// A WPF Markup extension which returns the name of a class's property member and validates that it is valid in case it is changed.  This is used by the LookupColumn to map it to a LookupColumnDefinition in a LookupDefinition.
    /// </summary>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    [ContentProperty(nameof(Member))]
    public class NameOfExtension : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the class type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }
        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <value>The member.</value>
        public string Member { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameOfExtension"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        public NameOfExtension(string member)
        {
            Member = member;
        }
        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.ArgumentNullException">serviceProvider</exception>
        /// <exception cref="System.ArgumentException">Syntax for x:NameOf is [propertyName], Type={x:Type [className]}</exception>
        /// <exception cref="System.ArgumentException">No property or field found for {Member} in {Type.Name}</exception>
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
                throw new ArgumentException($"No property or field found for {Member} in {Type.Name}");

            return Member;
        }
    }
}
