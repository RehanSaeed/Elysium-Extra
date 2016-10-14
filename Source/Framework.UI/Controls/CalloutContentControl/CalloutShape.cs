namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// A shape with a triangle shaped arrow on one corner.
    /// </summary>
    public sealed class CalloutShape : Shape
    {
        #region Dependency Properties
        
        public static readonly DependencyProperty ArrowAlignmentProperty = DependencyProperty.Register(
           "ArrowAlignment",
           typeof(ArrowAlignment),
           typeof(CalloutShape),
           new FrameworkPropertyMetadata(
               ArrowAlignment.Left,
               FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowHeightProperty = DependencyProperty.Register(
            "ArrowHeight",
            typeof(double),
            typeof(CalloutShape),
            new FrameworkPropertyMetadata(
                12.0D,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowPlacementProperty = DependencyProperty.Register(
            "ArrowPlacement",
            typeof(ArrowPlacement),
            typeof(CalloutShape),
            new FrameworkPropertyMetadata(
                ArrowPlacement.Top,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowWidthProperty = DependencyProperty.Register(
            "ArrowWidth",
            typeof(double),
            typeof(CalloutShape),
            new FrameworkPropertyMetadata(
                18.0D,
                FrameworkPropertyMetadataOptions.AffectsRender)); 

        #endregion

        private Rect rect;

        #region Public Properties

        /// <summary>
        /// Gets or sets the ArrowAlignment property.  This dependency property 
        /// indicates the offset of the arrow relative to its placement.
        /// <para/>
        /// If the arrow is placed along the top or bottom edge (ArrowPlacement = Top/Bottom), then 
        /// ArrowAlignment specifies whether the arrow is located on the left or right side of the
        /// edge.
        /// <para/>
        /// If the arrow is placed along the left or right edge (ArrowPlacement = Left/Right), then
        /// ArrowAlignment specifies whether the arrow is located on the top or bottom side of the
        /// edge.
        /// </summary>
        public ArrowAlignment ArrowAlignment
        {
            get { return (ArrowAlignment)this.GetValue(ArrowAlignmentProperty); }
            set { this.SetValue(ArrowAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ArrowHeight property.  This dependency property 
        /// indicates the Height of the arrow on the callout.
        /// </summary>
        public double ArrowHeight
        {
            get { return (double)this.GetValue(ArrowHeightProperty); }
            set { this.SetValue(ArrowHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ArrowWidth property.  This dependency property 
        /// indicates the width of the arrow on the callout.
        /// </summary>
        public double ArrowWidth
        {
            get { return (double)this.GetValue(ArrowWidthProperty); }
            set { this.SetValue(ArrowWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ArrowPlacement property.  This dependency property specifies which 
        /// edge the arrow is placed on (Top, Bottom, Left, Right).
        /// </summary>
        public ArrowPlacement ArrowPlacement
        {
            get { return (ArrowPlacement)this.GetValue(ArrowPlacementProperty); }
            set { this.SetValue(ArrowPlacementProperty, value); }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets a value that represents the <see cref="T:System.Windows.Media.Geometry" /> of the <see cref="T:System.Windows.Shapes.Shape" />.
        /// </summary>
        /// <returns>The <see cref="T:System.Windows.Media.Geometry" /> of the <see cref="T:System.Windows.Shapes.Shape" />.</returns>
        protected override Geometry DefiningGeometry
        {
            get
            {
                // Calculate the geometry for the rectangle.
                double top = Math.Max(0, this.rect.Top);
                double left = Math.Max(0, this.rect.Left);
                double width = Math.Max(0, this.rect.Width);
                double height = Math.Max(0, this.rect.Height);

                switch (ArrowPlacement)
                {
                    case ArrowPlacement.Top: 
                        top += this.ArrowHeight;
                        height -= this.ArrowHeight; 
                        break;
                    case ArrowPlacement.Bottom:
                        height -= this.ArrowHeight; 
                        break;
                }

                height = Math.Max(height, 0);
                width = Math.Max(width, 0);

                RectangleGeometry rectangle = new RectangleGeometry(new Rect(left, top, width, height), 0, 0);

                // If the width or height of the arrow is 0 then we can return the rectangle we just created
                if ((this.ArrowWidth == 0) || (this.ArrowHeight == 0) || (this.ArrowPlacement == ArrowPlacement.None))
                {
                    return rectangle;
                }

                // Otherwise, we calculate the geometry for the arrow
                Point p1, p2, p3;
                bool isLeft = this.ArrowAlignment == ArrowAlignment.Left;
                double s = this.GetStrokeThickness();

                switch (this.ArrowPlacement)
                {
                    case ArrowPlacement.Top:
                        if (this.ArrowAlignment == ArrowAlignment.Left)
                        {
                            p1 = new Point(s / 2, this.ArrowHeight + (s / 2));
                            p2 = new Point(s / 2, 0);
                            p3 = new Point(this.ArrowWidth, this.ArrowHeight + s);
                        }
                        else if (this.ArrowAlignment == ArrowAlignment.Right)
                        {
                            p1 = new Point(width - (this.ArrowWidth + s / 2), this.ArrowHeight + s);
                            p2 = new Point(width + s / 2, 0);
                            p3 = new Point(width + s / 2, this.ArrowHeight + s);
                        }
                        else
                        {
                            p1 = new Point();
                            p2 = new Point();
                            p3 = new Point();
                        }

                        break;

                    case ArrowPlacement.Bottom:
                        if (this.ArrowAlignment == ArrowAlignment.Left)
                        {
                            p1 = new Point(s / 2, height - s);
                            p2 = new Point(s / 2, height + this.ArrowHeight);
                            p3 = new Point(this.ArrowWidth, height - (s / 2));
                        }
                        else if (this.ArrowAlignment == ArrowAlignment.Right)
                        {
                            p1 = new Point(width - (this.ArrowWidth + this.StrokeThickness / 2), height - s);
                            p2 = new Point(width + s / 2, height + this.ArrowHeight);
                            p3 = new Point(width + s / 2, height - s);
                        }
                        else
                        {
                            p1 = new Point((width / 2) + (this.ArrowWidth / 2), height - s);
                            p2 = new Point(width / 2, height + this.ArrowHeight);
                            p3 = new Point((width / 2) - (this.ArrowWidth / 2), height - s / 2);
                        }

                        break;

                    default:
                        p1 = new Point();
                        p2 = new Point();
                        p3 = new Point();
                        break;
                }

                // Now create the arrow geometry with the points we've already calculated
                PathFigure p = new PathFigure();
                p.StartPoint = p1;

                p.Segments = new PathSegmentCollection();
                p.Segments.Add(new LineSegment(p1, false));
                p.Segments.Add(new LineSegment(p2, true));
                p.Segments.Add(new LineSegment(p3, true));
                p.Segments.Add(new LineSegment(p1, false));

                PathGeometry arrow = new PathGeometry();
                arrow.Figures.Add(p);

                // Combine the geometries and return
                return new CombinedGeometry(GeometryCombineMode.Union, arrow, rectangle);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Arranges a <see cref="T:System.Windows.Shapes.Shape" /> by evaluating its <see cref="P:System.Windows.Shapes.Shape.RenderedGeometry" /> and <see cref="P:System.Windows.Shapes.Shape.Stretch" /> properties.
        /// </summary>
        /// <param name="finalSize">The final evaluated size of the <see cref="T:System.Windows.Shapes.Shape" />.</param>
        /// <returns>
        /// The final size of the arranged <see cref="T:System.Windows.Shapes.Shape" /> element.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double penThickness = this.GetStrokeThickness();
            double margin = penThickness / 2;

            this.rect = new Rect(
                margin, 
                margin,
                Math.Max(0, finalSize.Width - penThickness),
                Math.Max(0, finalSize.Height - penThickness));

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// Measures a <see cref="T:System.Windows.Shapes.Shape" /> during the first layout pass prior to arranging it.
        /// </summary>
        /// <param name="constraint">A maximum <see cref="T:System.Windows.Size" /> to not exceed.</param>
        /// <returns>
        /// The maximum <see cref="T:System.Windows.Size" /> for the <see cref="T:System.Windows.Shapes.Shape" />.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            return this.GetNaturalSize();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the natural size of this instance.
        /// </summary>
        /// <returns>The natural size.</returns>
        private Size GetNaturalSize()
        {
            double strokeThickness = this.GetStrokeThickness();
            return new Size(strokeThickness, strokeThickness);
        }

        /// <summary>
        /// Gets the stroke thickness.
        /// </summary>
        /// <returns>The stroke thickness.</returns>
        private double GetStrokeThickness()
        {
            double strokeThickness = this.StrokeThickness;
            bool isPenNoOp = true;
            if ((this.Stroke != null) && !double.IsNaN(strokeThickness))
            {
                isPenNoOp = strokeThickness == 0;
            }

            return isPenNoOp ? 0.0d : Math.Abs(strokeThickness);
        }

        #endregion
    }
}
