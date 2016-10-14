namespace Framework.UI.Controls
{
    using System;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A <see cref="StackPanel"/> like panel which does not resize when an item changes it's size.
    /// </summary>
    internal sealed class StackCanvas : Canvas
    {
        #region Dependency Properties

        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register(
            "HorizontalContentAlignment",
            typeof(HorizontalAlignment),
            typeof(StackCanvas),
            new UIPropertyMetadata(HorizontalAlignment.Left));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation",
            typeof(Orientation),
            typeof(StackCanvas),
            new UIPropertyMetadata(Orientation.Vertical));

        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register(
            "VerticalContentAlignment",
            typeof(VerticalAlignment),
            typeof(StackCanvas),
            new UIPropertyMetadata(VerticalAlignment.Top));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the horizontal content alignment.
        /// </summary>
        /// <value>
        /// The horizontal content alignment.
        /// </value>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)this.GetValue(HorizontalContentAlignmentProperty); }
            set { this.SetValue(HorizontalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public Orientation Orientation
        {
            get { return (Orientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical content alignment.
        /// </summary>
        /// <value>
        /// The vertical content alignment.
        /// </value>
        public VerticalAlignment VerticalContentAlignment
        {
            get { return (VerticalAlignment)this.GetValue(VerticalContentAlignmentProperty); }
            set { this.SetValue(VerticalContentAlignmentProperty, value); }
        }

        #endregion

        #region Canvas Members

        /// <summary>
        /// Arranges the content of a <see cref="T:System.Windows.Controls.Canvas"/> element.
        /// </summary>
        /// <param name="arrangeSize">The size that this <see cref="T:System.Windows.Controls.Canvas"/> element should use to arrange its child elements.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Size"/> that represents the arranged size of this <see cref="T:System.Windows.Controls.Canvas"/> element and its descendants.
        /// </returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (ListBoxItem element in this.Children)
            {
                if (this.Orientation == Orientation.Vertical)
                {
                    switch (this.HorizontalContentAlignment)
                    {
                        case HorizontalAlignment.Left:
                            Canvas.SetLeft(element, 0);
                            Canvas.SetRight(element, double.NaN);
                            element.RenderTransformOrigin = new Point(0, 0);
                            break;
                        case HorizontalAlignment.Right:
                            Canvas.SetLeft(element, double.NaN);
                            Canvas.SetRight(element, 0);
                            element.RenderTransformOrigin = new Point(1, 0);
                            break;
                        case HorizontalAlignment.Center:
                        case HorizontalAlignment.Stretch:
                            Canvas.SetLeft(element, (this.ActualWidth / 2) - (element.DesiredSize.Width / 2));
                            Canvas.SetRight(element, double.NaN);
                            element.RenderTransformOrigin = new Point(0.5, 0);
                            break;
                        default:
                            throw new ArgumentException("Invalid Horizontal Alignment.");
                    }

                    Canvas.SetTop(element, (double)new ListIndexConverter().Convert(element, null, "Top", null));
                }
                else
                {
                    switch (this.VerticalContentAlignment)
                    {
                        case VerticalAlignment.Top:
                            Canvas.SetTop(element, 0);
                            Canvas.SetBottom(element, double.NaN);
                            element.RenderTransformOrigin = new Point(0, 0);
                            break;
                        case VerticalAlignment.Bottom:
                            Canvas.SetTop(element, double.NaN);
                            Canvas.SetBottom(element, 0);
                            element.RenderTransformOrigin = new Point(0, 1);
                            break;
                        case VerticalAlignment.Center:
                        case VerticalAlignment.Stretch:
                            Canvas.SetTop(element, (this.ActualHeight / 2) - (element.DesiredSize.Height / 2));
                            Canvas.SetBottom(element, double.NaN);
                            element.RenderTransformOrigin = new Point(0, 0.5);
                            break;
                        default:
                            throw new ArgumentException("Invalid Vertical Alignment.");
                    }

                    Canvas.SetLeft(element, (double)new ListIndexConverter().Convert(element, null, "Left", null));
                }

                if (!element.IsMouseOver)
                {
                    Canvas.SetZIndex(element, (int)new ListIndexConverter().Convert(element, null, "ZIndex", null));
                }
                else
                {
                    Canvas.SetZIndex(element, int.MaxValue);
                }
            }

            return base.ArrangeOverride(arrangeSize);
        }

        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement"/>-derived class.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>
        /// The size that this element determines it needs during layout, based on its calculations of child element sizes.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

            double curX = 0, curY = 0, curLineHeight = 0, curLineWidth = 0;

            foreach (UIElement child in this.Children)
            {
                child.Measure(infiniteSize);

                if (this.Orientation == Orientation.Horizontal)
                {
                    curX += child.DesiredSize.Width;
                    if (child.DesiredSize.Height > curLineHeight)
                    {
                        curLineHeight = child.DesiredSize.Height;
                    }
                }
                else
                {
                    curY += child.DesiredSize.Height;
                    if (child.DesiredSize.Width > curLineWidth)
                    {
                        curLineWidth = child.DesiredSize.Width;
                    }
                }
            }

            curX += curLineWidth;
            curY += curLineHeight;

            Size resultSize = new Size();
            resultSize.Width = double.IsPositiveInfinity(availableSize.Width)
                ? curX : availableSize.Width;
            resultSize.Height = double.IsPositiveInfinity(availableSize.Height)
                ? curY : availableSize.Height;

            return resultSize;
        }

        #endregion
    }
}
