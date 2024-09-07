// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="WPFExtensionMethods.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO.Packaging;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using RingSoft.DataEntryControls.Engine;

// ReSharper disable PossibleNullReferenceException

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// WPF lookup controls extension methods.
    /// </summary>
    internal static class WpfExtensionMethods
    {
        /// <summary>
        /// Loads the view from URI.
        /// </summary>
        /// <param name="userControl">The user control.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void LoadViewFromUri(this UserControl userControl, string baseUri)
        {
            try
            {
                var resourceLocater = new Uri(baseUri, UriKind.Relative);
                var exprCa = (PackagePart)typeof(Application).GetMethod("GetResourceOrContentPart", BindingFlags.NonPublic | BindingFlags.Static)?.Invoke(null, new object[] { resourceLocater });
                if (exprCa != null)
                {
                    var stream = exprCa.GetStream();
                    var uri = new Uri((Uri)typeof(BaseUriHelper).GetProperty("PackAppBaseUri", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null) ?? throw new InvalidOperationException(), resourceLocater);
                    var parserContext = new ParserContext
                    {
                        BaseUri = uri
                    };
                    typeof(XamlReader).GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static)?.Invoke(null, new object[] { stream, parserContext, userControl, true });
                }
            }
            catch (Exception)
            {
                //log
            }
        }
    }
}
