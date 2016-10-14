namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The smart columns panel.
    /// </summary>
    public sealed class SmartColumnsPanel : Panel
    {
        #region Dependency Properties

        public static readonly DependencyProperty AvailableWidthProperty = DependencyProperty.Register(
            "AvailableWidth",
            typeof(double),
            typeof(SmartColumnsPanel),
            new FrameworkPropertyMetadata(0.0D, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.Register(
            "ColumnCount",
            typeof(int),
            typeof(SmartColumnsPanel),
            new PropertyMetadata(0));

        public static readonly DependencyProperty MinColumnWidthProperty = DependencyProperty.RegisterAttached(
            "MinColumnWidth",
            typeof(double),
            typeof(SmartColumnsPanel),
            new PropertyMetadata(400D));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the available width.
        /// </summary>
        public double AvailableWidth
        {
            get { return (double)this.GetValue(AvailableWidthProperty); }
            set { this.SetValue(AvailableWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the column count.
        /// </summary>
        public int ColumnCount
        {
            get { return (int)this.GetValue(ColumnCountProperty); }
            set { this.SetValue(ColumnCountProperty, value); }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The get min column width.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <returns> The <see cref="double"/>. </returns>
        public static double GetMinColumnWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(MinColumnWidthProperty);
        }

        /// <summary>
        /// The set min column width.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <param name="value"> The value. </param>
        public static void SetMinColumnWidth(DependencyObject obj, double value)
        {
            obj.SetValue(MinColumnWidthProperty, value);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The arrange override.
        /// </summary>
        /// <param name="finalSize"> The final size. </param>
        /// <returns> The <see cref="Size"/>. </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double top = 0;
            double prevChildHeight = 0;
            double totalColumnWidth = 0;
            double currentTabWidthAvailable = 0;

            foreach (UIElement child in this.Children)
            {
                double childColumnWidth = this.GetColumnWidth(child);

                if (top == 0)
                {
                    child.Arrange(new Rect(totalColumnWidth, top, childColumnWidth, child.DesiredSize.Height));
                    totalColumnWidth += this.AvailableWidth;
                    top += child.DesiredSize.Height;
                    currentTabWidthAvailable = this.AvailableWidth - childColumnWidth;
                    prevChildHeight = child.DesiredSize.Height;
                }
                else if (top > 0 && (top + child.DesiredSize.Height > finalSize.Height)
                     && (childColumnWidth > currentTabWidthAvailable | (top - prevChildHeight + child.DesiredSize.Height) > finalSize.Height))
                {
                    top = 0;
                    child.Arrange(new Rect(totalColumnWidth, top, childColumnWidth, child.DesiredSize.Height));
                    totalColumnWidth += this.AvailableWidth;
                    top = child.DesiredSize.Height;
                    currentTabWidthAvailable = this.AvailableWidth - childColumnWidth;
                    prevChildHeight = child.DesiredSize.Height;
                }
                else
                {
                    if (childColumnWidth <= currentTabWidthAvailable)
                    {
                        top -= prevChildHeight;
                        child.Arrange(new Rect(totalColumnWidth - currentTabWidthAvailable, top, childColumnWidth, child.DesiredSize.Height));
                        currentTabWidthAvailable -= childColumnWidth;
                        prevChildHeight = Math.Max(prevChildHeight, child.DesiredSize.Height);
                        top += prevChildHeight;
                    }
                    else
                    {
                        // top += prevChildHeight;
                        child.Arrange(new Rect(totalColumnWidth - this.AvailableWidth, top, childColumnWidth, child.DesiredSize.Height));
                        top += child.DesiredSize.Height;

                        // prevChildHeight += child.DesiredSize.Height;
                        prevChildHeight = child.DesiredSize.Height;
                        currentTabWidthAvailable = this.AvailableWidth - childColumnWidth;
                    }
                }
            }

            return new Size(totalColumnWidth, finalSize.Height);
        }

        /// <summary>
        /// The measure override.
        /// </summary>
        /// <param name="availableSize"> The available size. </param>
        /// <returns> The <see cref="Size"/>. </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // Avoid exception when availableSize.Height is Infinity
            availableSize.Height = Math.Min(SystemParameters.FullPrimaryScreenHeight, availableSize.Height);

            double top = 0;
            double totalColumnWidth = 0;
            double prevChildHeight = 0;
            int columnCount = 0;
            double currentTabWidthAvailable = 0;

            foreach (UIElement child in this.Children)
            {
                double childColumnWidth = this.GetColumnWidth(child);

                child.Measure(new Size(childColumnWidth, availableSize.Height));

                if (top == 0)
                {
                    // totalColumnWidth += childColumnWidth;
                    totalColumnWidth += this.AvailableWidth;
                    top += child.DesiredSize.Height;
                    ++columnCount;
                    currentTabWidthAvailable = this.AvailableWidth - childColumnWidth;
                    prevChildHeight = child.DesiredSize.Height;
                }
                else if (top > 0 && (top + child.DesiredSize.Height > availableSize.Height)
                     && (childColumnWidth > currentTabWidthAvailable | (top - prevChildHeight + child.DesiredSize.Height) > availableSize.Height))
                {
                    // top = 0;
                    top = child.DesiredSize.Height;

                    // totalColumnWidth += childColumnWidth;
                    totalColumnWidth += this.AvailableWidth;
                    ++columnCount;
                    currentTabWidthAvailable = this.AvailableWidth - childColumnWidth;
                    prevChildHeight = child.DesiredSize.Height;
                }
                else
                {
                    if (childColumnWidth <= currentTabWidthAvailable)
                    {
                        top -= prevChildHeight;
                        currentTabWidthAvailable -= childColumnWidth;
                        prevChildHeight = Math.Max(prevChildHeight, child.DesiredSize.Height);
                        top += prevChildHeight;
                    }
                    else
                    {
                        // top += prevChildHeight;
                        top += child.DesiredSize.Height;

                        // prevChildHeight += child.DesiredSize.Height;
                        prevChildHeight = child.DesiredSize.Height;
                        currentTabWidthAvailable = this.AvailableWidth - childColumnWidth;
                    }
                }
            }

            this.ColumnCount = columnCount;

            return new Size(totalColumnWidth, availableSize.Height);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The get column width.
        /// </summary>
        /// <param name="child"> The child. </param>
        /// <returns> The <see cref="double"/>. </returns>
        private double GetColumnWidth(UIElement child)
        {
            double columnWidth = GetMinColumnWidth(child);
            double count = Math.Max(Math.Floor(this.AvailableWidth / columnWidth), 1);
            double width = this.AvailableWidth / count;
            return width;
        }

        #endregion
    }
}
