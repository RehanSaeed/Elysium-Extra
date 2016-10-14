namespace Framework.UI.Controls
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// A border that clips its contents.
    /// </summary>
    public class ClippingBorder : ContentControl
    {
        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ClippingBorder), new PropertyMetadata(new PropertyChangedCallback(CornerRadius_Changed)));

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(ClippingBorder), new PropertyMetadata(new PropertyChangedCallback(ClipContent_Changed)));

        /// <summary>
        /// Stores the top left content control.
        /// </summary>
        private ContentControl topLeftContentControl;

        /// <summary>
        /// Stores the top right content control.
        /// </summary>
        private ContentControl topRightContentControl;

        /// <summary>
        /// Stores the bottom right content control.
        /// </summary>
        private ContentControl bottomRightContentControl;

        /// <summary>
        /// Stores the bottom left content control.
        /// </summary>
        private ContentControl bottomLeftContentControl;

        /// <summary>
        /// Stores the clip responsible for clipping the top left corner.
        /// </summary>
        private RectangleGeometry topLeftClip;

        /// <summary>
        /// Stores the clip responsible for clipping the top right corner.
        /// </summary>
        private RectangleGeometry topRightClip;

        /// <summary>
        /// Stores the clip responsible for clipping the bottom right corner.
        /// </summary>
        private RectangleGeometry bottomRightClip;

        /// <summary>
        /// Stores the clip responsible for clipping the bottom left corner.
        /// </summary>
        private RectangleGeometry bottomLeftClip;

        /// <summary>
        /// Stores the main border.
        /// </summary>
        private Border border;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClippingBorder"/> class.
        /// </summary>
        public ClippingBorder()
        {
            this.DefaultStyleKey = typeof(ClippingBorder);
            this.SizeChanged += new SizeChangedEventHandler(this.ClippingBorder_SizeChanged);
        }

        /// <summary>
        /// Gets or sets the border corner radius.
        /// This is a thickness, as there is a problem parsing CornerRadius types.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)this.GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content is clipped.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool ClipContent
        {
            get { return (bool)this.GetValue(ClipContentProperty); }
            set { this.SetValue(ClipContentProperty, value); }
        }

        /// <summary>
        /// Gets the UI elements out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.border = this.GetTemplateChild("PART_Border") as Border;
            this.topLeftContentControl = this.GetTemplateChild("PART_TopLeftContentControl") as ContentControl;
            this.topRightContentControl = this.GetTemplateChild("PART_TopRightContentControl") as ContentControl;
            this.bottomRightContentControl = this.GetTemplateChild("PART_BottomRightContentControl") as ContentControl;
            this.bottomLeftContentControl = this.GetTemplateChild("PART_BottomLeftContentControl") as ContentControl;

            if (this.topLeftContentControl != null)
            {
                this.topLeftContentControl.SizeChanged += new SizeChangedEventHandler(this.ContentControl_SizeChanged);
            }

            this.topLeftClip = this.GetTemplateChild("PART_TopLeftClip") as RectangleGeometry;
            this.topRightClip = this.GetTemplateChild("PART_TopRightClip") as RectangleGeometry;
            this.bottomRightClip = this.GetTemplateChild("PART_BottomRightClip") as RectangleGeometry;
            this.bottomLeftClip = this.GetTemplateChild("PART_BottomLeftClip") as RectangleGeometry;

            this.UpdateClipContent(this.ClipContent);

            this.UpdateCornerRadius(this.CornerRadius);
        }

        /// <summary>
        /// Sets the corner radius.
        /// </summary>
        /// <param name="newCornerRadius">The new corner radius.</param>
        internal void UpdateCornerRadius(CornerRadius newCornerRadius)
        {
            if (this.border != null)
            {
                this.border.CornerRadius = newCornerRadius;
            }

            if (this.topLeftClip != null)
            {
                this.topLeftClip.RadiusX = this.topLeftClip.RadiusY = newCornerRadius.TopLeft - (Math.Min(this.BorderThickness.Left, this.BorderThickness.Top) / 2);
            }

            if (this.topRightClip != null)
            {
                this.topRightClip.RadiusX = this.topRightClip.RadiusY = newCornerRadius.TopRight - (Math.Min(this.BorderThickness.Top, this.BorderThickness.Right) / 2);
            }

            if (this.bottomRightClip != null)
            {
                this.bottomRightClip.RadiusX = this.bottomRightClip.RadiusY = newCornerRadius.BottomRight - (Math.Min(this.BorderThickness.Right, this.BorderThickness.Bottom) / 2);
            }

            if (this.bottomLeftClip != null)
            {
                this.bottomLeftClip.RadiusX = this.bottomLeftClip.RadiusY = newCornerRadius.BottomLeft - (Math.Min(this.BorderThickness.Bottom, this.BorderThickness.Left) / 2);
            }

            this.UpdateClipSize(new Size(this.ActualWidth, this.ActualHeight));
        }

        /// <summary>
        /// Updates whether the content is clipped.
        /// </summary>
        /// <param name="clipContent">Whether the content is clipped.</param>
        internal void UpdateClipContent(bool clipContent)
        {
            if (clipContent)
            {
                if (this.topLeftContentControl != null)
                {
                    this.topLeftContentControl.Clip = this.topLeftClip;
                }

                if (this.topRightContentControl != null)
                {
                    this.topRightContentControl.Clip = this.topRightClip;
                }

                if (this.bottomRightContentControl != null)
                {
                    this.bottomRightContentControl.Clip = this.bottomRightClip;
                }

                if (this.bottomLeftContentControl != null)
                {
                    this.bottomLeftContentControl.Clip = this.bottomLeftClip;
                }

                this.UpdateClipSize(new Size(this.ActualWidth, this.ActualHeight));
            }
            else
            {
                if (this.topLeftContentControl != null)
                {
                    this.topLeftContentControl.Clip = null;
                }

                if (this.topRightContentControl != null)
                {
                    this.topRightContentControl.Clip = null;
                }

                if (this.bottomRightContentControl != null)
                {
                    this.bottomRightContentControl.Clip = null;
                }

                if (this.bottomLeftContentControl != null)
                {
                    this.bottomLeftContentControl.Clip = null;
                }
            }
        }

        /// <summary>
        /// Updates the corner radius.
        /// </summary>
        /// <param name="dependencyObject">The clipping border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void CornerRadius_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ClippingBorder clippingBorder = (ClippingBorder)dependencyObject;
            clippingBorder.UpdateCornerRadius((CornerRadius)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the content clipping.
        /// </summary>
        /// <param name="dependencyObject">The clipping border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void ClipContent_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ClippingBorder clippingBorder = (ClippingBorder)dependencyObject;
            clippingBorder.UpdateClipContent((bool)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the clips.
        /// </summary>
        /// <param name="sender">The clipping border</param>
        /// <param name="e">Size Changed Event Args.</param>
        private void ClippingBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ClipContent)
            {
                this.UpdateClipSize(e.NewSize);
            }
        }

        /// <summary>
        /// Updates the clip size.
        /// </summary>
        /// <param name="sender">A content control.</param>
        /// <param name="e">Size Changed Event Args</param>
        private void ContentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ClipContent)
            {
                this.UpdateClipSize(new Size(this.ActualWidth, this.ActualHeight));
            }
        }

        /// <summary>
        /// Updates the clip size.
        /// </summary>
        /// <param name="size">The control size.</param>
        private void UpdateClipSize(Size size)
        {
            if (size.Width > 0 || size.Height > 0)
            {
                double contentWidth = Math.Max(0, size.Width - this.BorderThickness.Left - this.BorderThickness.Right);
                double contentHeight = Math.Max(0, size.Height - this.BorderThickness.Top - this.BorderThickness.Bottom);

                if (this.topLeftClip != null)
                {
                    this.topLeftClip.Rect = new Rect(0, 0, contentWidth + (this.CornerRadius.TopLeft * 2), contentHeight + (this.CornerRadius.TopLeft * 2));
                }

                if (this.topRightClip != null)
                {
                    this.topRightClip.Rect = new Rect(0 - this.CornerRadius.TopRight, 0, contentWidth + this.CornerRadius.TopRight, contentHeight + this.CornerRadius.TopRight);
                }

                if (this.bottomRightClip != null)
                {
                    this.bottomRightClip.Rect = new Rect(0 - this.CornerRadius.BottomRight, 0 - this.CornerRadius.BottomRight, contentWidth + this.CornerRadius.BottomRight, contentHeight + this.CornerRadius.BottomRight);
                }

                if (this.bottomLeftClip != null)
                {
                    this.bottomLeftClip.Rect = new Rect(0, 0 - this.CornerRadius.BottomLeft, contentWidth + this.CornerRadius.BottomLeft, contentHeight + this.CornerRadius.BottomLeft);
                }
            }
        }
    }
}
