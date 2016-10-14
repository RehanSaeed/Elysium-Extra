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
    /// A border that also shows a radial shadow below it.
    /// </summary>
    public class RadialShadowBorder : ContentControl
    {
        /// <summary>
        /// The drop shadow colour property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowColorProperty =
            DependencyProperty.Register("RadialShadowColor", typeof(Color), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(RadialShadowColor_Changed)));

        /// <summary>
        /// The drop shadow opacity property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowOpacityProperty =
            DependencyProperty.Register("RadialShadowOpacity", typeof(double), typeof(RadialShadowBorder), null);

        /// <summary>
        /// The drop shadow distance property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowVerticalOffsetProperty =
            DependencyProperty.Register("RadialShadowVerticalOffset", typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(RadialShadowVerticalOffset_Changed)));

        /// <summary>
        /// The drop shadow angle property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowWidthProperty =
            DependencyProperty.Register("RadialShadowWidth", typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(RadialShadowWidth_Changed)));

        /// <summary>
        /// The drop shadow spread property.
        /// </summary>
        public static readonly DependencyProperty RadialShadowSpreadProperty =
            DependencyProperty.Register("RadialShadowSpread", typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(RadialShadowSpread_Changed)));

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RadialShadowBorder), null);

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(RadialShadowBorder), null);

        /// <summary>
        /// The inner shadow gradient stop.
        /// </summary>
        private GradientStop shadowInnerStop;

        /// <summary>
        /// The outer shadow gradient stop.
        /// </summary>
        private GradientStop shadowOuterStop;

        /// <summary>
        /// Stores the shadow translate transform.
        /// </summary>
        private TranslateTransform shadowTranslate;

        /// <summary>
        /// Stores the shadow scale;
        /// </summary>
        private ScaleTransform shadowScale;

        /// <summary>
        /// Stores the shadow.
        /// </summary>
        private Ellipse shadow;

        /// <summary>
        /// Initialises a new instance of the <see cref="RadialShadowBorder"/> class.
        /// </summary>
        public RadialShadowBorder()
        {
            this.DefaultStyleKey = typeof(RadialShadowBorder);
            this.SizeChanged += new SizeChangedEventHandler(this.RadialShadowBorder_SizeChanged);
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
        /// Gets or sets the drop shadow colour.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow color.")]
        public Color RadialShadowColor
        {
            get { return (Color)this.GetValue(RadialShadowColorProperty); }
            set { this.SetValue(RadialShadowColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow opacity.")]
        public double RadialShadowOpacity
        {
            get { return (double)this.GetValue(RadialShadowOpacityProperty); }
            set { this.SetValue(RadialShadowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow distance.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow vertical offset.")]
        public double RadialShadowVerticalOffset
        {
            get { return (double)this.GetValue(RadialShadowVerticalOffsetProperty); }
            set { this.SetValue(RadialShadowVerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow angle.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow width.")]
        public double RadialShadowWidth
        {
            get { return (double)this.GetValue(RadialShadowWidthProperty); }
            set { this.SetValue(RadialShadowWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow spread.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow spread.")]
        public double RadialShadowSpread
        {
            get { return (double)this.GetValue(RadialShadowSpreadProperty); }
            set { this.SetValue(RadialShadowSpreadProperty, value); }
        }

        /// <summary>
        /// Gets the parts out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.shadow = (Ellipse)this.GetTemplateChild("PART_Shadow");
            this.shadowInnerStop = (GradientStop)this.GetTemplateChild("PART_ShadowInnerStop");
            this.shadowOuterStop = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop");
            this.shadowTranslate = (TranslateTransform)this.GetTemplateChild("PART_ShadowTranslate");
            this.shadowScale = (ScaleTransform)this.GetTemplateChild("PART_ShadowScale");

            this.UpdateShadowSize(new Size(this.ActualWidth, this.ActualHeight));
            this.UpdateStops(this.RadialShadowSpread);
            this.UpdateShadowScale(this.RadialShadowWidth);
            this.UpdateShadowVerticalOffset(this.RadialShadowVerticalOffset);
        }

        /// <summary>
        /// Updates the shadow size.
        /// </summary>
        /// <param name="newSize">The new control size.</param>
        internal void UpdateShadowSize(Size newSize)
        {
            if (this.shadow != null)
            {
                this.shadow.Height = newSize.Height / 3;
                this.shadow.Margin = new Thickness(0, 0, 0, -this.shadow.Height / 2);
            }
        }

        /// <summary>
        /// Updates the shadow scale;
        /// </summary>
        /// <param name="scaleX">The scale X of the shadow.</param>
        internal void UpdateShadowScale(double scaleX)
        {
            if (this.shadowScale != null)
            {
                this.shadowScale.ScaleX = scaleX;
            }
        }

        /// <summary>
        /// Updates the gradient stops offset.
        /// </summary>
        /// <param name="spread">The spread of the stops.</param>
        internal void UpdateStops(double spread)
        {
            if (this.shadowInnerStop != null)
            {
                this.shadowInnerStop.Offset = spread;
            }
        }

        /// <summary>
        /// Updates the shadow colour.
        /// </summary>
        /// <param name="color">The new shadow colour.</param>
        internal void UpdateShadowColor(Color color)
        {
             if (this.shadowInnerStop != null)
            {
                this.shadowInnerStop.Color = color;
            }

            if (this.shadowOuterStop != null)
            {
                this.shadowOuterStop.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }
        }

        /// <summary>
        /// Updates the vertical offset of the shadow.
        /// </summary>
        /// <param name="verticalOffset">The vertical offset.</param>
        internal void UpdateShadowVerticalOffset(double verticalOffset)
        {
            if (this.shadowTranslate != null)
            {
                this.shadowTranslate.Y = Math.Max(0, this.RadialShadowVerticalOffset);
            }
        }

        /// <summary>
        /// Updates the radial shadow.
        /// </summary>
        /// <param name="dependencyObject">The radial shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void RadialShadowVerticalOffset_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            RadialShadowBorder radialShadowBorder = (RadialShadowBorder)dependencyObject;
            radialShadowBorder.UpdateShadowVerticalOffset((double)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the radial shadow.
        /// </summary>
        /// <param name="dependencyObject">The radial shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void RadialShadowWidth_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            RadialShadowBorder radialShadowBorder = (RadialShadowBorder)dependencyObject;
            radialShadowBorder.UpdateShadowScale((double)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the radial shadow.
        /// </summary>
        /// <param name="dependencyObject">The radial shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void RadialShadowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            RadialShadowBorder radialShadowBorder = (RadialShadowBorder)dependencyObject;
            radialShadowBorder.UpdateShadowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the radial shadow.
        /// </summary>
        /// <param name="dependencyObject">The radial shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void RadialShadowSpread_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            RadialShadowBorder radialShadowBorder = (RadialShadowBorder)dependencyObject;
            radialShadowBorder.UpdateStops((double)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the radial shadow size.
        /// </summary>
        /// <param name="sender">The radial shadow border.</param>
        /// <param name="e">Size changed event args.</param>
        private void RadialShadowBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateShadowSize(e.NewSize);
        }
    }
}
