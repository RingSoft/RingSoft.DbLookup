// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="GridViewSort.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class GridViewSort.
    /// </summary>
    internal class GridViewSort
    {
        #region Public attached properties

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        /// <summary>
        /// Sets the name of the property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The property name property
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new UIPropertyMetadata(null)
            );

        /// <summary>
        /// Gets the show sort glyph.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetShowSortGlyph(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSortGlyphProperty);
        }

        /// <summary>
        /// Sets the show sort glyph.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetShowSortGlyph(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSortGlyphProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowSortGlyph.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The show sort glyph property
        /// </summary>
        public static readonly DependencyProperty ShowSortGlyphProperty =
            DependencyProperty.RegisterAttached("ShowSortGlyph", typeof(bool), typeof(GridViewSort), new UIPropertyMetadata(true));

        /// <summary>
        /// Gets the sort glyph ascending.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>ImageSource.</returns>
        public static ImageSource GetSortGlyphAscending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphAscendingProperty);
        }

        /// <summary>
        /// Sets the sort glyph ascending.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetSortGlyphAscending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphAscendingProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortGlyphAscending.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The sort glyph ascending property
        /// </summary>
        public static readonly DependencyProperty SortGlyphAscendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphAscending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets the sort glyph descending.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>ImageSource.</returns>
        public static ImageSource GetSortGlyphDescending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphDescendingProperty);
        }

        /// <summary>
        /// Sets the sort glyph descending.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetSortGlyphDescending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphDescendingProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortGlyphDescending.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The sort glyph descending property
        /// </summary>
        public static readonly DependencyProperty SortGlyphDescendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphDescending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Private attached properties

        /// <summary>
        /// Gets the sorted column header.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>GridViewColumnHeader.</returns>
        private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject obj)
        {
            return (GridViewColumnHeader)obj.GetValue(SortedColumnHeaderProperty);
        }

        /// <summary>
        /// Sets the sorted column header.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        private static void SetSortedColumnHeader(DependencyObject obj, GridViewColumnHeader value)
        {
            obj.SetValue(SortedColumnHeaderProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortedColumn.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The sorted column header property
        /// </summary>
        private static readonly DependencyProperty SortedColumnHeaderProperty =
            DependencyProperty.RegisterAttached("SortedColumnHeader", typeof(GridViewColumnHeader), typeof(GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Helper methods

        /// <summary>
        /// Applies the sort.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="listView">The list view.</param>
        /// <param name="sortedColumnHeader">The sorted column header.</param>
        public static void ApplySort(ListSortDirection direction, ListView listView, GridViewColumnHeader sortedColumnHeader)
        {
            GridViewColumnHeader currentSortedColumnHeader = GetSortedColumnHeader(listView);
            if (currentSortedColumnHeader != null)
            {
                RemoveSortGlyph(currentSortedColumnHeader);
                RemoveSortGlyph(sortedColumnHeader);
            }
            if (GetShowSortGlyph(listView))
                AddSortGlyph(
                    sortedColumnHeader,
                    direction,
                    direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView));
            SetSortedColumnHeader(listView, sortedColumnHeader);
        }

        /// <summary>
        /// Adds the sort glyph.
        /// </summary>
        /// <param name="columnHeader">The column header.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="sortGlyph">The sort glyph.</param>
        private static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            if (adornerLayer != null)
                adornerLayer.Add(
                    new SortGlyphAdorner(
                        columnHeader,
                        direction,
                        sortGlyph
                    ));
        }

        /// <summary>
        /// Adds the non primary sort glyph.
        /// </summary>
        /// <param name="columnHeader">The column header.</param>
        /// <param name="sortIndex">Index of the sort.</param>
        public static void AddNonPrimarySortGlyph(GridViewColumnHeader columnHeader, int sortIndex)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            if (adornerLayer != null)
                adornerLayer.Add(
                    new SortGlyphAdorner(
                        columnHeader,
                        sortIndex
                    ));
        }

        /// <summary>
        /// Removes the sort glyph.
        /// </summary>
        /// <param name="columnHeader">The column header.</param>
        public static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(columnHeader);
                if (adorners != null)
                {
                    foreach (Adorner adorner in adorners)
                    {
                        if (adorner is SortGlyphAdorner)
                            adornerLayer.Remove(adorner);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the size of the glyph.
        /// </summary>
        /// <param name="columnHeader">The column header.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="listView">The list view.</param>
        /// <returns>Size.</returns>
        public static Size GetGlyphSize(GridViewColumnHeader columnHeader, ListSortDirection direction, ListView listView)
        {
            var glyph = direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView);
            if (glyph == null)
            {
                var adorner = new SortGlyphAdorner(columnHeader, direction, null);
                var defaultGlyph = adorner.GetDefaultGlyph();
                return new Size(defaultGlyph.Bounds.Width, defaultGlyph.Bounds.Height);
            }

            return new Size(glyph.Width, glyph.Height);
        }

        #endregion

        #region SortGlyphAdorner nested class

        /// <summary>
        /// Class SortGlyphAdorner.
        /// Implements the <see cref="Adorner" />
        /// </summary>
        /// <seealso cref="Adorner" />
        private class SortGlyphAdorner : Adorner
        {
            /// <summary>
            /// The column header
            /// </summary>
            private GridViewColumnHeader _columnHeader;
            /// <summary>
            /// The direction
            /// </summary>
            private ListSortDirection _direction;
            /// <summary>
            /// The sort glyph
            /// </summary>
            private ImageSource _sortGlyph;
            /// <summary>
            /// The sort index
            /// </summary>
            private int _sortIndex;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortGlyphAdorner" /> class.
            /// </summary>
            /// <param name="columnHeader">The column header.</param>
            /// <param name="direction">The direction.</param>
            /// <param name="sortGlyph">The sort glyph.</param>
            public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
                : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _direction = direction;
                _sortGlyph = sortGlyph;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortGlyphAdorner" /> class.
            /// </summary>
            /// <param name="columnHeader">The column header.</param>
            /// <param name="sortIndex">Index of the sort.</param>
            public SortGlyphAdorner(GridViewColumnHeader columnHeader, int sortIndex) : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _sortIndex = sortIndex;
            }

            /// <summary>
            /// Gets the default glyph.
            /// </summary>
            /// <returns>Geometry.</returns>
            public Geometry GetDefaultGlyph()
            {
                double x1 = _columnHeader.ActualWidth / 2 - 5;
                double x2 = x1 + 10;
                double x3 = x1 + 5;
                double y1 = 3;
                double y2 = y1 + 5;

                if (_direction == ListSortDirection.Ascending)
                {
                    double tmp = y1;
                    y1 = y2;
                    y2 = tmp;
                }

                PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
                pathSegmentCollection.Add(new LineSegment(new Point(x2, y1), true));
                pathSegmentCollection.Add(new LineSegment(new Point(x3, y2), true));

                PathFigure pathFigure = new PathFigure(
                    new Point(x1, y1),
                    pathSegmentCollection,
                    true);

                PathFigureCollection pathFigureCollection = new PathFigureCollection();
                pathFigureCollection.Add(pathFigure);

                PathGeometry pathGeometry = new PathGeometry(pathFigureCollection);
                return pathGeometry;
            }

            /// <summary>
            /// Called when [render].
            /// </summary>
            /// <param name="drawingContext">The drawing context.</param>
            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (_sortIndex > 0)
                {
                    double x = _columnHeader.ActualWidth / 2 - 5;
                    double y = 0;
                    Rect rect = new Rect(x, y, 10, 10);
                    drawingContext.DrawText(
                        new FormattedText(_sortIndex.ToString(),
                            CultureInfo.GetCultureInfo("en-us"),
                            FlowDirection.LeftToRight,
                            new Typeface(_columnHeader.FontFamily, _columnHeader.FontStyle, _columnHeader.FontWeight,
                                _columnHeader.FontStretch),
                            10, Brushes.Black, new NumberSubstitution(), 1.0),
                        rect.Location);
                }
                else
                {
                    if (_sortGlyph != null)
                    {
                        double x = _columnHeader.ActualWidth / 2 - 8;
                        double y = 3;
                        Rect rect = new Rect(x, y, 16, 16);
                        drawingContext.DrawImage(_sortGlyph, rect);
                    }
                    else
                    {
                        drawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0), GetDefaultGlyph());
                    }
                }
            }
        }

        #endregion
    }
}
