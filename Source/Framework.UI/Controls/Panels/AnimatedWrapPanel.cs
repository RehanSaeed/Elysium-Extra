namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    /// <summary>
    /// An animated version of the <see cref="WrapPanel"/>.
    /// </summary>
    public sealed class AnimatedWrapPanel : Panel
    {
        #region Dependency Properties

        public static readonly DependencyProperty AnimationEasingFunctionProperty = DependencyProperty.Register(
            "AnimationEasingFunction", 
            typeof(IEasingFunction), 
            typeof(AnimatedWrapPanel), 
            new PropertyMetadata(new QuinticEase()));

        public static readonly DependencyProperty AnimationStartXProperty = DependencyProperty.Register(
            "AnimationStartX",
            typeof(double),
            typeof(AnimatedWrapPanel),
            new PropertyMetadata(-100D));

        public static readonly DependencyProperty AnimationStartYProperty = DependencyProperty.Register(
            "AnimationStartY",
            typeof(double),
            typeof(AnimatedWrapPanel),
            new PropertyMetadata(0D));

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register(
            "AnimationTime",
            typeof(TimeSpan),
            typeof(AnimatedWrapPanel),
            new PropertyMetadata(TimeSpan.FromSeconds(1)));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation",
            typeof(Orientation),
            typeof(AnimatedWrapPanel),
            new PropertyMetadata(Orientation.Horizontal)); 

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the animation easing function.
        /// </summary>
        /// <value>
        /// The animation easing function.
        /// </value>
        public IEasingFunction AnimationEasingFunction
        {
            get { return (IEasingFunction)this.GetValue(AnimationEasingFunctionProperty); }
            set { this.SetValue(AnimationEasingFunctionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the animation time.
        /// </summary>
        /// <value>
        /// The animation time.
        /// </value>
        public TimeSpan AnimationTime
        {
            get { return (TimeSpan)this.GetValue(AnimationTimeProperty); }
            set { this.SetValue(AnimationTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the starting X co-ordinate of an element when it is first loaded.
        /// </summary>
        /// <value>
        /// The animation start X co-ordinate.
        /// </value>
        public double AnimationStartX
        {
            get { return (double)this.GetValue(AnimationStartXProperty); }
            set { this.SetValue(AnimationStartXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the starting Y co-ordinate of an element when it is first loaded.
        /// </summary>
        /// <value>
        /// The animation start Y co-ordinate.
        /// </value>
        public double AnimationStartY
        {
            get { return (double)this.GetValue(AnimationStartYProperty); }
            set { this.SetValue(AnimationStartYProperty, value); }
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

        #endregion

        #region Protected Methods

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement" /> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.Children == null || this.Children.Count == 0)
            {
                return finalSize;
            }

            double curX = 0, curY = 0, curLineHeight = 0;

            foreach (UIElement child in this.Children)
            {
                TranslateTransform translateTransform = child.RenderTransform as TranslateTransform;

                if (translateTransform == null)
                {
                    child.RenderTransformOrigin = new Point(0, 0);
                    translateTransform = new TranslateTransform(this.AnimationStartX, this.AnimationStartY);
                    child.RenderTransform = translateTransform;
                }

                if (((curX + child.DesiredSize.Width) > finalSize.Width) || 
                    (this.Orientation == Orientation.Vertical))
                {
                    // Wrap to next line
                    curY += curLineHeight;
                    curX = 0;
                    curLineHeight = 0;
                }

                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

                translateTransform.BeginAnimation(
                    TranslateTransform.XProperty,
                    new DoubleAnimation(curX, this.AnimationTime)
                    {
                        EasingFunction = this.AnimationEasingFunction
                    },
                    HandoffBehavior.Compose);
                translateTransform.BeginAnimation(
                    TranslateTransform.YProperty, 
                    new DoubleAnimation(curY, this.AnimationTime)
                    {
                        EasingFunction = this.AnimationEasingFunction
                    }, 
                    HandoffBehavior.Compose);

                curX += child.DesiredSize.Width;

                if (child.DesiredSize.Height > curLineHeight)
                {
                    curLineHeight = child.DesiredSize.Height;
                }
            }

            return finalSize;
        }

        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement" />-derived class.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>
        /// The size that this element determines it needs during layout, based on its calculations of child element sizes.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            double curX = 0, curY = 0, curLineHeight = 0;

            foreach (UIElement child in this.Children)
            {
                child.Measure(infiniteSize);

                if (((curX + child.DesiredSize.Width) > availableSize.Width) || 
                    (this.Orientation == Orientation.Vertical))
                {
                    // Wrap to next line
                    curY += curLineHeight;
                    curX = 0;
                    curLineHeight = 0;
                }

                curX += child.DesiredSize.Width;

                if (child.DesiredSize.Height > curLineHeight)
                {
                    curLineHeight = child.DesiredSize.Height;
                }
            }

            curY += curLineHeight;

            Size resultSize = new Size(
                double.IsPositiveInfinity(availableSize.Width) ? curX : availableSize.Width,
                double.IsPositiveInfinity(availableSize.Height) ? curY : availableSize.Height);

            return resultSize;
        } 

        #endregion
    }
}
