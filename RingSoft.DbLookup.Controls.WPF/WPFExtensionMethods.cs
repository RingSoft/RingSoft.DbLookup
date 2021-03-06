﻿using System;
using System.IO.Packaging;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
// ReSharper disable PossibleNullReferenceException

namespace RingSoft.DbLookup.Controls.WPF
{
    internal static class WpfExtensionMethods
    {
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
