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
    /// A border that also shows a perspective shadow.
    /// </summary>
    public class PerspectiveShadowBorder : ContentControl
    {
        /// <summary>
        /// The perspective shadow colour property.
        /// </summary>
        public static readonly DependencyProperty PerspectiveShadowColorProperty =
            DependencyProperty.Register("PerspectiveShadowColor", typeof(Color), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(PerspectiveShadowColor_Changed)));

        /// <summary>
        /// The perspective shadow opacity property.
        /// </summary>
        public static readonly DependencyProperty PerspectiveShadowOpacityProperty =
            DependencyProperty.Register("PerspectiveShadowOpacity", typeof(double), typeof(PerspectiveShadowBorder), null);

        /// <summary>
        /// The Perspective shadow angle property.
        /// </summary>
        public static readonly DependencyProperty PerspectiveShadowAngleProperty =
            DependencyProperty.Register("PerspectiveShadowAngle", typeof(double), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(PerspectiveShadowAngle_Changed)));

        /// <summary>
        /// The Perspective shadow spread property.
        /// </summary>
        public static readonly DependencyProperty PerspectiveShadowSpreadProperty =
            DependencyProperty.Register("PerspectiveShadowSpread", typeof(double), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(PerspectiveShadowSpread_Changed)));

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PerspectiveShadowBorder), null);

        /// <summary>
        /// The shadow corner radius property.
        /// </summary>
        public static readonly DependencyProperty ShadowCornerRadiusProperty =
            DependencyProperty.Register("ShadowCornerRadius", typeof(CornerRadius), typeof(PerspectiveShadowBorder), null);

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(PerspectiveShadowBorder), null);

        /// <summary>
        /// Stores the skew transform.
        /// </summary>
        private SkewTransform skewTransform;

        /// <summary>
        /// Stores the scale transform.
        /// </summary>
        private ScaleTransform scaleTransform;

        /// <summary>
        /// The top out gradient stop.
        /// </summary>
        private GradientStop shadowOuterStop1;

        /// <summary>
        /// The bottom outer gradient stop.
        /// </summary>
        private GradientStop shadowOuterStop2;

        /// <summary>
        /// Stores the top gradient stop.
        /// </summary>
        private GradientStop shadowVertical1;

        /// <summary>
        /// Stores the bottom gradient stop.
        /// </summary>
        private GradientStop shadowVertical2;

        /// <summary>
        /// Stores the left gradient stop.
        /// </summary>
        private GradientStop shadowHorizontal1;

        /// <summary>
        /// Stores the right gradient stop.
        /// </summary>
        private GradientStop shadowHorizontal2;

        /// <summary>
        /// Initialises a new instance of the <see cref="PerspectiveShadowBorder"/> class.
        /// </summary>
        public PerspectiveShadowBorder()
        {
            this.DefaultStyleKey = typeof(PerspectiveShadowBorder);
            this.SizeChanged += new SizeChangedEventHandler(this.PerspectiveShadowBorder_SizeChanged);
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
            get
            {
                return (CornerRadius)this.GetValue(CornerRadiusProperty);
            }

            set
            {
                this.SetValue(CornerRadiusProperty, value);

                CornerRadius shadowCornerRadius = new CornerRadius(
                    Math.Abs(value.TopLeft * 1.5),
                    Math.Abs(value.TopRight * 1.5),
                    Math.Abs(value.BottomRight * 1.5),
                    Math.Abs(value.BottomLeft * 1.5));
                this.SetValue(ShadowCornerRadiusProperty, shadowCornerRadius);
            }
        }

        /// <summary>
        /// Gets or sets the Perspective shadow colour.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("the Perspective shadow color.")]
        public Color PerspectiveShadowColor
        {
            get { return (Color)this.GetValue(PerspectiveShadowColorProperty); }
            set { this.SetValue(PerspectiveShadowColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Perspective shadow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow opacity.")]
        public double PerspectiveShadowOpacity
        {
            get { return (double)this.GetValue(PerspectiveShadowOpacityProperty); }
            set { this.SetValue(PerspectiveShadowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Perspective shadow angle.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow angle.")]
        public double PerspectiveShadowAngle
        {
            get { return (double)this.GetValue(PerspectiveShadowAngleProperty); }
            set { this.SetValue(PerspectiveShadowAngleProperty, Math.Max(Math.Min(value, 89), -89)); }
        }

        /// <summary>
        /// Gets or sets the Perspective shadow spread.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow spread.")]
        public double PerspectiveShadowSpread
        {
            get { return (double)this.GetValue(PerspectiveShadowSpreadProperty); }
            set { this.SetValue(PerspectiveShadowSpreadProperty, value); }
        }

        /// <summary>
        /// Gets the parts from the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.skewTransform = (SkewTransform)this.GetTemplateChild("PART_PerspectiveShadowSkewTransform");
            this.scaleTransform = (ScaleTransform)this.GetTemplateChild("PART_PerspectiveShadowScaleTransform");
            this.shadowOuterStop1 = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop1");
            this.shadowOuterStop2 = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop2");
            this.shadowVertical1 = (GradientStop)this.GetTemplateChild("PART_ShadowVertical1");
            this.shadowVertical2 = (GradientStop)this.GetTemplateChild("PART_ShadowVertical2");
            this.shadowHorizontal1 = (GradientStop)this.GetTemplateChild("PART_ShadowHorizontal1");
            this.shadowHorizontal2 = (GradientStop)this.GetTemplateChild("PART_ShadowHorizontal2");

            this.UpdateStops(new Size(this.ActualWidth, this.ActualHeight));
            this.UpdateShadowAngle(this.PerspectiveShadowAngle);
        }

        /// <summary>
        /// Updates the gradient stops.
        /// </summary>
        /// <param name="size">The size of the control.</param>
        internal void UpdateStops(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                if (this.shadowHorizontal1 != null)
                {
                    this.shadowHorizontal1.Offset = this.PerspectiveShadowSpread / (size.Width + (this.PerspectiveShadowSpread * 2));
                }

                if (this.shadowHorizontal2 != null)
                {
                    this.shadowHorizontal2.Offset = 1 - (this.PerspectiveShadowSpread / (size.Width + (this.PerspectiveShadowSpread * 2)));
                }

                if (this.shadowVertical1 != null)
                {
                    this.shadowVertical1.Offset = this.PerspectiveShadowSpread / (size.Height + (this.PerspectiveShadowSpread * 2));
                }

                if (this.shadowVertical2 != null)
                {
                    this.shadowVertical2.Offset = 1 - (this.PerspectiveShadowSpread / (size.Height + (this.PerspectiveShadowSpread * 2)));
                }
            }
        }

        /// <summary>
        /// Updates the Perspective shadow colour.
        /// </summary>
        /// <param name="color">The new colour.</param>
        internal void UpdatePerspectiveShadowColor(Color color)
        {
            if (this.shadowVertical1 != null)
            {
                this.shadowVertical1.Color = color;
            }

            if (this.shadowVertical2 != null)
            {
                this.shadowVertical2.Color = color;
            }

            if (this.shadowOuterStop1 != null)
            {
                this.shadowOuterStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }

            if (this.shadowOuterStop2 != null)
            {
                this.shadowOuterStop2.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }
        }

        /// <summary>
        /// Updates the shadow angle.
        /// </summary>
        /// <param name="newAngle">The new angle</param>
        internal void UpdateShadowAngle(double newAngle)
        {
            if (this.skewTransform != null)
            {
                this.skewTransform.AngleX = Math.Min(Math.Max(newAngle, -45), 45);
            }

            if (this.scaleTransform != null)
            {
                this.scaleTransform.ScaleY = 1 - (Math.Abs(this.PerspectiveShadowAngle) / 89.0);
            }
        }

        /// <summary>
        /// Updates the Perspective shadow.
        /// </summary>
        /// <param name="dependencyObject">The Perspective shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void PerspectiveShadowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            PerspectiveShadowBorder perspectiveShadowBorder = (PerspectiveShadowBorder)dependencyObject;
            perspectiveShadowBorder.UpdatePerspectiveShadowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the Perspective shadow.
        /// </summary>
        /// <param name="dependencyObject">The Perspective shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void PerspectiveShadowSpread_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            PerspectiveShadowBorder perspectiveShadowBorder = (PerspectiveShadowBorder)dependencyObject;
            perspectiveShadowBorder.UpdateStops(new Size(perspectiveShadowBorder.ActualWidth, perspectiveShadowBorder.ActualHeight));
        }

        /// <summary>
        /// Updates the Perspective shadow.
        /// </summary>
        /// <param name="dependencyObject">The Perspective shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void PerspectiveShadowAngle_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            PerspectiveShadowBorder perspectiveShadowBorder = (PerspectiveShadowBorder)dependencyObject;
            perspectiveShadowBorder.UpdateShadowAngle((double)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the stops.
        /// </summary>
        /// <param name="sender">The Perspective shadow border.</param>
        /// <param name="e">Size changed event args.</param>
        private void PerspectiveShadowBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateStops(e.NewSize);
        }
    }
}
