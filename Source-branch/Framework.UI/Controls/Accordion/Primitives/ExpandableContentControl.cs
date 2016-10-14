namespace Framework.UI.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Represents a control with a single piece of content that expands or 
    /// collapses in a sliding motion to a specified desired size.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplatePart(Name = ElementContentSiteName, Type = typeof(ContentPresenter))]
    public class ExpandableContentControl : ContentControl
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the Percentage dependency property.
        /// </summary>
        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register(
            "Percentage",
            typeof(double),
            typeof(ExpandableContentControl),
            new FrameworkPropertyMetadata(0.0, OnPercentagePropertyChanged, CoercePercentageProperty));

        public static readonly DependencyProperty RecalculateOnSizeChangedProperty = DependencyProperty.Register(
            "RecalculateOnSizeChanged",
            typeof(bool),
            typeof(ExpandableContentControl),
            new PropertyMetadata(false));

        /// <summary>
        /// Identifies the RevealMode dependency property.
        /// </summary>
        public static readonly DependencyProperty RevealModeProperty = DependencyProperty.Register(
            "RevealMode",
            typeof(ExpandDirection),
            typeof(ExpandableContentControl),
            new PropertyMetadata(ExpandDirection.Down, OnRevealModePropertyChanged));

        /// <summary>
        /// Identifies the TargetSize dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetSizeProperty = DependencyProperty.Register(
            "TargetSize",
            typeof(Size),
            typeof(ExpandableContentControl),
            new PropertyMetadata(new Size(double.NaN, double.NaN), OnTargetSizePropertyChanged));

        #endregion

        #region Fields

        /// <summary>
        /// The name of the ContentSite template part.
        /// </summary>
        private const string ElementContentSiteName = "ContentSite";

        /// <summary>
        /// The Geometry used to clip this control. The control will potentially
        /// have less available space than the content it is arranging. That
        /// part will be clipped.
        /// </summary>
        private readonly RectangleGeometry clippingRectangle;

        private double? calculatePercentage;

        /// <summary>
        /// BackingField for the ContentSite property.
        /// </summary>
        private ContentPresenter contentSite;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="ExpandableContentControl"/> class.
        /// </summary>
        static ExpandableContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandableContentControl), new FrameworkPropertyMetadata(typeof(ExpandableContentControl)));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ExpandableContentControl"/> class.
        /// </summary>
        public ExpandableContentControl()
        {
            this.clippingRectangle = new RectangleGeometry();
            this.Clip = this.clippingRectangle;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when the content changed its size.
        /// </summary>
        public event SizeChangedEventHandler ContentSizeChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the relative percentage of the content that is 
        /// currently visible. A percentage of 1 corresponds to the complete
        /// TargetSize.
        /// </summary>
        public double Percentage
        {
            get { return (double)this.GetValue(PercentageProperty); }
            set { this.SetValue(PercentageProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to recalculate the percentage on size changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if recalculate on size changed; otherwise, <c>false</c>.
        /// </value>
        public bool RecalculateOnSizeChanged
        {
            get { return (bool)this.GetValue(RecalculateOnSizeChangedProperty); }
            set { this.SetValue(RecalculateOnSizeChangedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction in which the ExpandableContentControl 
        /// content window opens.
        /// </summary>
        public ExpandDirection RevealMode
        {
            get { return (ExpandDirection)this.GetValue(RevealModeProperty); }
            set { this.SetValue(RevealModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the desired size of the ExpandableContentControl content.
        /// </summary>
        /// <remarks>Use the percentage property to animate to this size.</remarks>
        public Size TargetSize
        {
            get { return (Size)this.GetValue(TargetSizeProperty); }
            set { this.SetValue(TargetSizeProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the content current visible size.
        /// </summary>
        internal Size RelevantContentSize
        {
            get { return new Size(this.IsHorizontalRevealMode ? Width : 0, this.IsVerticalRevealMode ? Height : 0); }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the ContentSite template part.
        /// </summary>
        private ContentPresenter ContentSite
        {
            get
            {
                return this.contentSite;
            }

            set
            {
                if (this.contentSite != null)
                {
                    this.contentSite.SizeChanged -= this.OnContentSiteSizeChanged;

                    if (this.contentSite.Content != null && this.contentSite.Content is FrameworkElement)
                    {
                        ((FrameworkElement)this.contentSite.Content).SizeChanged -= this.OnContentSiteSizeChanged;
                    }
                }

                this.contentSite = value;

                if (this.contentSite != null)
                {
                    this.contentSite.SizeChanged += this.OnContentSiteSizeChanged;

                    if (this.contentSite.Content != null && this.contentSite.Content is FrameworkElement)
                    {
                        ((FrameworkElement)this.contentSite.Content).SizeChanged += this.OnContentSiteSizeChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the content should be revealed horizontally.
        /// </summary>
        private bool IsHorizontalRevealMode
        {
            get { return this.RevealMode == ExpandDirection.Left || this.RevealMode == ExpandDirection.Right; }
        }

        /// <summary>
        /// Gets a value indicating whether the content should be revealed vertically.
        /// </summary>
        private bool IsVerticalRevealMode
        {
            get { return this.RevealMode == ExpandDirection.Down || this.RevealMode == ExpandDirection.Up; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds the visual tree for the ExpandableContentControl control when a 
        /// new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.ContentSite = this.GetTemplateChild(ElementContentSiteName) as ContentPresenter;

            this.SetRevealDimension();

            this.SetNonRevealDimension();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Interprets TargetSize.
        /// </summary>
        /// <returns>A size that can be safely used to measure content.</returns>
        internal Size CalculateDesiredContentSize()
        {
            Size desiredSize = new Size();

            if (this.IsHorizontalRevealMode)
            {
                desiredSize.Height = this.contentSite.DesiredSize.Height;
                desiredSize.Width = double.PositiveInfinity;

                if (desiredSize.Height == 0)
                {
                    desiredSize.Height = double.PositiveInfinity;
                }
            }
            else if (this.IsVerticalRevealMode)
            {
                desiredSize.Height = double.PositiveInfinity;
                desiredSize.Width = this.contentSite.DesiredSize.Width;

                if (desiredSize.Width == 0)
                {
                    desiredSize.Width = double.PositiveInfinity;
                }
            }

            return desiredSize;
        }

        /// <summary>
        /// Measures the content with a specific size.
        /// </summary>
        /// <param name="desiredSize">The size passed to the content.</param>
        internal void MeasureContent(Size desiredSize)
        {
            if (this.ContentSite != null)
            {
                this.ContentSite.Measure(desiredSize);
            }
        }

        /// <summary>
        /// Recalculates the percentage based on a new size.
        /// </summary>
        /// <param name="value">The new size used to base percentages on.</param>
        internal void RecalculatePercentage(Size value)
        {
            this.Percentage = CalculatePercentage(this, value);
        }

        /// <summary>
        /// Recalculates the percentage based on a new size.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        internal void RecalculatePercentage(double percentage)
        {
            this.calculatePercentage = percentage;
            this.CoerceValue(PercentageProperty);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Arranges the control and its content. Content is arranged according
        /// to the TargetSize and clipped.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this 
        /// object should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Log("Before Arrange ");

            if (this.ContentSite == null)
            {
                return finalSize;
            }

            // arrange content as if filling the targetSize
            Size desiredSize = this.TargetSize;

            // if a direction is not set, use given size.
            if (desiredSize.Width.Equals(double.NaN))
            {
                desiredSize.Width = this.Width;

                if (desiredSize.Width.Equals(double.NaN))
                {
                    if (this.IsVerticalRevealMode)
                    {
                        desiredSize.Width = this.ActualWidth;
                    }
                    else
                    {
                        desiredSize.Width = this.DesiredSize.Width;
                    }
                }
            }

            if (desiredSize.Height.Equals(double.NaN))
            {
                desiredSize.Height = this.Height;

                if (desiredSize.Height.Equals(double.NaN))
                {
                    if (this.IsVerticalRevealMode)
                    {
                        desiredSize.Height = this.ActualHeight;
                    }
                    else
                    {
                        desiredSize.Height = this.DesiredSize.Height;
                    }
                }
            }

            Rect finalRect = new Rect(new Point(0, 0), desiredSize);
            this.ContentSite.Arrange(finalRect);

            Size actualSize = new Size(this.IsHorizontalRevealMode ? this.Width : finalSize.Width, this.IsVerticalRevealMode ? this.Height : finalSize.Height);

            if (double.IsNaN(actualSize.Width))
            {
                actualSize.Width = finalSize.Width;
            }

            if (double.IsNaN(actualSize.Height))
            {
                actualSize.Height = finalSize.Height;
            }

            this.UpdateClip(actualSize);

            this.Log("After  Arrange ");

            return actualSize;
        }

        /// <summary>
        /// Does a measure pass of the control and its content. The content will
        /// get measured according to the TargetSize and is clipped.
        /// </summary>
        /// <param name="availableSize">The available size that this object can 
        /// give to child objects. Infinity can be specified as a value to 
        /// indicate that the object will size to whatever content is available.</param>
        /// <returns>
        /// The size that this object determines it needs during layout, based 
        /// on its calculations of child object allotted sizes.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.Log("Before Measure ");

            // this control will always follow the TargetSize
            // and allow its content to take all the space it needs
            if (this.ContentSite != null)
            {
                // we will adhere to the available size, to allow scrollbars
                // to appear                
                Size desiredSize = availableSize;
                if (this.Percentage != 1)
                {
                    // we shall use the targetsize, to allow the content
                    // to adjust to the final size it should be.
                    desiredSize = this.CalculateDesiredContentSize();
                }

                this.MeasureContent(desiredSize);

                this.Log("After  Measure ");

                return this.ContentSite.DesiredSize;
            }

            this.Log("After  Measure ");

            return new Size(0, 0);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Recalculates the percentage based on a new size.
        /// </summary>
        /// <param name="expandableContentControl">The control which is going to be evaluated</param>
        /// <param name="value">The new size used to base percentages on.</param>
        /// <returns>The new percentage value</returns>
        private static double CalculatePercentage(ExpandableContentControl expandableContentControl, Size value)
        {
            double newPercentage = 0.0;

            if (expandableContentControl.ContentSite != null)
            {
                if (expandableContentControl.IsVerticalRevealMode)
                {
                    newPercentage = expandableContentControl.ActualHeight / (double.IsNaN(value.Height) ? expandableContentControl.ContentSite.DesiredSize.Height : value.Height);
                }
                else if (expandableContentControl.IsHorizontalRevealMode)
                {
                    newPercentage = expandableContentControl.ActualWidth / (double.IsNaN(value.Width) ? expandableContentControl.ContentSite.DesiredSize.Width : value.Width);
                }
            }

            return newPercentage;
        }

        /// <summary>
        /// Coerce the percentage property as necessary.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns>The coerced value.</returns>
        private static object CoercePercentageProperty(DependencyObject dependencyObject, object baseValue)
        {
            // This works around a subtle difference between WPF 3 and SL 3,
            // where a value assigned to a dependency property from within the control
            // is considered to be from a local source, and trumps styles/templates from that point forward.
            // This could be worked around in WPF 4.0 with DependencyObject.SetCurrentValue()
            object returnValue = baseValue;
            ExpandableContentControl expandableContentControl = dependencyObject as ExpandableContentControl;

            // This method can be called by a parent control through RecalculatePercentage(double)
            // To know when to coerce and avoid having to do so more than needed, calculatePercentage can have 3 states:
            // Null: Don't corce using internal value, simply return baseValue
            // Double.NAN: Coerce by computing new percentage value based on current target size
            // value: Corece by returning explicit value previously set in RecalculatePercentage(double)
            if (expandableContentControl != null && expandableContentControl.calculatePercentage.HasValue)
            {
                if (double.IsNaN(expandableContentControl.calculatePercentage.Value))
                {
                    returnValue = CalculatePercentage(expandableContentControl, expandableContentControl.TargetSize);
                }
                else
                {
                    returnValue = expandableContentControl.calculatePercentage.Value;
                }

                expandableContentControl.calculatePercentage = null;
            }

            return returnValue;
        }

        /// <summary>
        /// PercentageProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">Page that changed its Percentage.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnPercentagePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ExpandableContentControl source = (ExpandableContentControl)dependencyObject;
            source.SetRevealDimension();
            source.InvalidateMeasure();
        }

        /// <summary>
        /// RevealModeProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">ExpandableContentControl that changed its RevealMode.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnRevealModePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ExpandableContentControl source = (ExpandableContentControl)dependencyObject;
            ExpandDirection value = (ExpandDirection)e.NewValue;

            if (value != ExpandDirection.Down &&
                value != ExpandDirection.Left &&
                value != ExpandDirection.Right &&
                value != ExpandDirection.Up)
            {
                // revert to old value
                source.RevealMode = (ExpandDirection)e.OldValue;

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    AccordionResources.Expander_OnExpandDirectionPropertyChanged_InvalidValue,
                    value);
                throw new ArgumentException(message, "e");
            }

            // set the non-reveal dimension
            source.SetNonRevealDimension();

            // calculate the reveal dimension
            source.SetRevealDimension();
        }

        /// <summary>
        /// TargetSizeProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">ExpandableContentControl that changed its TargetSize.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnTargetSizePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ExpandableContentControl source = (ExpandableContentControl)dependencyObject;
            Size value = (Size)e.NewValue;

            // recalculate percentage based on this new targetsize.
            // for instance, percentage was at 1 and width was 100. Now width was changed to 200, this means
            // that the percentage needs to be set to 0.5 so that a reveal animation can be started again.

            // We are essentially trying to re-evaluate the percentage property based on another property having changed,
            // which is exactly what the coerce functionality of dependency properties is for
            source.RecalculatePercentage(double.NaN);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Raises the ContentSizeChanged event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnContentSiteSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Log("Before Content ");

            if (this.RecalculateOnSizeChanged)
            {
                this.MeasureContent(this.CalculateDesiredContentSize());
                this.RecalculatePercentage(this.TargetSize);

                // set the non-reveal dimension
                this.SetNonRevealDimension();

                // calculate the reveal dimension
                this.SetRevealDimension();
            }

            SizeChangedEventHandler handler = this.ContentSizeChanged;

            if (handler != null)
            {
                handler(this, e);
            }

            this.Log("After  Content ");
        }

        /// <summary>
        /// Sets the opposite dimension.
        /// </summary>
        private void SetNonRevealDimension()
        {
            if (this.IsHorizontalRevealMode)
            {
                // reset height to target size height. This can be double.NaN (auto size)
                this.Height = this.TargetSize.Height;
            }

            if (this.IsVerticalRevealMode)
            {
                // reset width to target size width. This can be double.NaN (auto size)
                this.Width = this.TargetSize.Width;
            }
        }

        /// <summary>
        /// Sets the dimensions according to the current percentage.
        /// </summary>
        private void SetRevealDimension()
        {
            if (this.ContentSite == null)
            {
                return;
            }

            if (this.IsHorizontalRevealMode)
            {
                double targetWidth = this.TargetSize.Width;
                if (double.IsNaN(targetWidth))
                {
                    // NaN has the same meaning as autosize, which in this context means the desired size
                    targetWidth = this.ContentSite.DesiredSize.Width;
                }

                this.Width = this.Percentage * targetWidth;
            }

            if (this.IsVerticalRevealMode)
            {
                double targetHeight = this.TargetSize.Height;
                if (double.IsNaN(targetHeight))
                {
                    // NaN has the same meaning as autosize, which in this context means the desired size
                    targetHeight = this.ContentSite.DesiredSize.Height;
                }

                this.Height = this.Percentage * targetHeight;
            }
        }

        /// <summary>
        /// Updates the clip geometry.
        /// </summary>
        /// <param name="arrangeSize">Size of the visible part of the control.</param>
        private void UpdateClip(Size arrangeSize)
        {
            if (this.Clip != this.clippingRectangle)
            {
                this.Clip = this.clippingRectangle;
            }

            this.clippingRectangle.Rect = new Rect(0.0, 0.0, arrangeSize.Width, arrangeSize.Height);
        }

        /// <summary>
        /// Logs the specified message to the debug window.
        /// </summary>
        /// <param name="message">The message.</param>
        private void Log(string message)
        {
            // System.Diagnostics.Debug.WriteLine(
            //    message + string.Format(
            //    "{0:000.0}% ({1:000.0},{2:000.0}) ({3:000.0}:{4:000.0}) ({5:000.0}:{6:000.0}) ||| ({7:000.0},{8:000.0}) ({9:000.0}:{10:000.0}) ({11:000.0}:{12:000.0})",
            //    this.Percentage * 100,
            //    this.DesiredSize.Width, this.DesiredSize.Height,
            //    this.ActualWidth, this.ActualHeight,
            //    this.Width, this.Height,
            //    this.contentSite.DesiredSize.Width, this.contentSite.DesiredSize.Height,
            //    this.contentSite.ActualWidth, this.contentSite.ActualHeight,
            //    this.contentSite.Width, this.contentSite.Height));
        }

        #endregion
    }
}
