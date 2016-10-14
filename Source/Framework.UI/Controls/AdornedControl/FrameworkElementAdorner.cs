namespace Framework.UI.Controls
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// This class is an adorner that allows a FrameworkElement derived class to adorn another FrameworkElement.
    /// </summary>
    public class FrameworkElementAdorner : Adorner
    {
        #region Fields

        private FrameworkElement child;

        private AdornerPlacement horizontalAdornerPlacement = AdornerPlacement.Inside;
        private AdornerPlacement verticalAdornerPlacement = AdornerPlacement.Inside;

        private double offsetX = 0.0;
        private double offsetY = 0.0;

        private double positionX = double.NaN;
        private double positionY = double.NaN;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="FrameworkElementAdorner"/> class.
        /// </summary>
        /// <param name="adornerChildElement">The adorner child element.</param>
        /// <param name="adornedElement">The adorned element.</param>
        public FrameworkElementAdorner(
            FrameworkElement adornerChildElement,
            FrameworkElement adornedElement)
            : base(adornedElement)
        {
            this.child = adornerChildElement;

            this.AddLogicalChild(adornerChildElement);
            this.AddVisualChild(adornerChildElement);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="FrameworkElementAdorner"/> class.
        /// </summary>
        /// <param name="adornerChildElement">The adorner child element.</param>
        /// <param name="adornedElement">The adorned element.</param>
        /// <param name="horizontalAdornerPlacement">The horizontal adorner placement.</param>
        /// <param name="verticalAdornerPlacement">The vertical adorner placement.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        public FrameworkElementAdorner(
            FrameworkElement adornerChildElement,
            FrameworkElement adornedElement,
            AdornerPlacement horizontalAdornerPlacement,
            AdornerPlacement verticalAdornerPlacement,
            double offsetX,
            double offsetY)
            : base(adornedElement)
        {
            this.child = adornerChildElement;
            this.horizontalAdornerPlacement = horizontalAdornerPlacement;
            this.verticalAdornerPlacement = verticalAdornerPlacement;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            adornedElement.SizeChanged += new SizeChangedEventHandler(this.OnAdornedElementSizeChanged);

            this.AddLogicalChild(adornerChildElement);
            this.AddVisualChild(adornerChildElement);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="FrameworkElement"/> that this adorner is bound to.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The element that this adorner is bound to.
        /// The default value is null.
        /// </returns>
        public new FrameworkElement AdornedElement
        {
            get
            {
                return (FrameworkElement)base.AdornedElement;
            }
        }

        /// <summary>
        /// Gets or sets the X position of the child (when not set to NaN).
        /// </summary>
        /// <value>The X position.</value>
        public double PositionX
        {
            get { return this.positionX; }
            set { this.positionX = value; }
        }

        /// <summary>
        /// Gets or sets the Y position of the child (when not set to NaN).
        /// </summary>
        /// <value>The Y position.</value>
        public double PositionY
        {
            get { return this.positionY; }
            set { this.positionY = value; }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets an enumerator for logical child elements of this element.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An enumerator for logical child elements of this element.
        /// </returns>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(this.child);
                return (IEnumerator)list.GetEnumerator();
            }
        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of visual child elements for this element.
        /// </returns>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disconnect the child element from the visual tree so that it may be reused later.
        /// </summary>
        public void DisconnectChild()
        {
            this.RemoveLogicalChild(this.child);
            this.RemoveVisualChild(this.child);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = this.PositionX;
            if (double.IsNaN(x))
            {
                x = this.DetermineX();
            }

            double y = this.PositionY;
            if (double.IsNaN(y))
            {
                y = this.DetermineY();
            }

            double adornerWidth = this.DetermineWidth();
            double adornerHeight = this.DetermineHeight();
            this.child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            return finalSize;
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)"/>, and returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>
        /// The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.
        /// </returns>
        protected override Visual GetVisualChild(int index)
        {
            return this.child;
        }

        /// <summary>
        /// Implements any custom measuring behaviour for the adorner.
        /// </summary>
        /// <param name="constraint">A size to constrain the adorner to.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Size"/> object representing the amount of layout space needed by the adorner.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            this.child.Measure(constraint);
            return this.child.DesiredSize;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determine the X coordinate of the child.
        /// </summary>
        /// <returns>The X coordinate of the child.</returns>
        private double DetermineX()
        {
            switch (this.child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    {
                        if (this.horizontalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            return -this.child.DesiredSize.Width + this.offsetX;
                        }
                        else
                        {
                            return this.offsetX;
                        }
                    }

                case HorizontalAlignment.Right:
                    {
                        if (this.horizontalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            double adornedWidth = this.AdornedElement.ActualWidth;
                            return adornedWidth + this.offsetX;
                        }
                        else
                        {
                            double adornerWidth = this.child.DesiredSize.Width;
                            double adornedWidth = this.AdornedElement.ActualWidth;
                            double x = adornedWidth - adornerWidth;
                            return x + this.offsetX;
                        }
                    }

                case HorizontalAlignment.Center:
                    {
                        double adornerWidth = this.child.DesiredSize.Width;
                        double adornedWidth = this.AdornedElement.ActualWidth;
                        double x = (adornedWidth / 2) - (adornerWidth / 2);
                        return x + this.offsetX;
                    }

                case HorizontalAlignment.Stretch:
                    {
                        return 0.0;
                    }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the Y coordinate of the child.
        /// </summary>
        /// <returns>The Y coordinate of the child.</returns>
        private double DetermineY()
        {
            switch (this.child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    {
                        if (this.verticalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            return -this.child.DesiredSize.Height + this.offsetY;
                        }
                        else
                        {
                            return this.offsetY;
                        }
                    }

                case VerticalAlignment.Bottom:
                    {
                        if (this.verticalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            double adornedHeight = this.AdornedElement.ActualHeight;
                            return adornedHeight + this.offsetY;
                        }
                        else
                        {
                            double adornerHeight = this.child.DesiredSize.Height;
                            double adornedHeight = this.AdornedElement.ActualHeight;
                            double x = adornedHeight - adornerHeight;
                            return x + this.offsetY;
                        }
                    }

                case VerticalAlignment.Center:
                    {
                        double adornerHeight = this.child.DesiredSize.Height;
                        double adornedHeight = this.AdornedElement.ActualHeight;
                        double x = (adornedHeight / 2) - (adornerHeight / 2);
                        return x + this.offsetY;
                    }

                case VerticalAlignment.Stretch:
                    {
                        return 0.0;
                    }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the width of the child.
        /// </summary>
        /// <returns>The width of the child.</returns>
        private double DetermineWidth()
        {
            if (!double.IsNaN(this.PositionX))
            {
                return this.child.DesiredSize.Width;
            }

            switch (this.child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return this.child.DesiredSize.Width;
                case HorizontalAlignment.Right:
                    return this.child.DesiredSize.Width;
                case HorizontalAlignment.Center:
                    return this.child.DesiredSize.Width;
                case HorizontalAlignment.Stretch:
                    return this.AdornedElement.ActualWidth;
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the height of the child.
        /// </summary>
        /// <returns>The height of the child.</returns>
        private double DetermineHeight()
        {
            if (!double.IsNaN(this.PositionY))
            {
                return this.child.DesiredSize.Height;
            }

            switch (this.child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    return this.child.DesiredSize.Height;
                case VerticalAlignment.Bottom:
                    return this.child.DesiredSize.Height;
                case VerticalAlignment.Center:
                    return this.child.DesiredSize.Height;
                case VerticalAlignment.Stretch:
                    return this.AdornedElement.ActualHeight;
            }

            return 0.0;
        }

        /// <summary>
        /// Event raised when the adorned control's size has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void OnAdornedElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        #endregion
    }
}