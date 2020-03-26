using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RSDbLookup.Controls.Wpf
{
    public class GridViewSort
    {
        #region Public attached properties

        public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new UIPropertyMetadata(null)
            );

        public static bool GetShowSortGlyph(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSortGlyphProperty);
        }

        public static void SetShowSortGlyph(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSortGlyphProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowSortGlyph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSortGlyphProperty =
            DependencyProperty.RegisterAttached("ShowSortGlyph", typeof(bool), typeof(GridViewSort), new UIPropertyMetadata(true));

        public static ImageSource GetSortGlyphAscending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphAscendingProperty);
        }

        public static void SetSortGlyphAscending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphAscendingProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortGlyphAscending.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortGlyphAscendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphAscending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        public static ImageSource GetSortGlyphDescending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphDescendingProperty);
        }

        public static void SetSortGlyphDescending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphDescendingProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortGlyphDescending.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortGlyphDescendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphDescending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Private attached properties

        private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject obj)
        {
            return (GridViewColumnHeader)obj.GetValue(SortedColumnHeaderProperty);
        }

        private static void SetSortedColumnHeader(DependencyObject obj, GridViewColumnHeader value)
        {
            obj.SetValue(SortedColumnHeaderProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortedColumn.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty SortedColumnHeaderProperty =
            DependencyProperty.RegisterAttached("SortedColumnHeader", typeof(GridViewColumnHeader), typeof(GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Helper methods

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

        private class SortGlyphAdorner : Adorner
        {
            private GridViewColumnHeader _columnHeader;
            private ListSortDirection _direction;
            private ImageSource _sortGlyph;
            private int _sortIndex;

            public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
                : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _direction = direction;
                _sortGlyph = sortGlyph;
            }

            public SortGlyphAdorner(GridViewColumnHeader columnHeader, int sortIndex) : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _sortIndex = sortIndex;
            }

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
