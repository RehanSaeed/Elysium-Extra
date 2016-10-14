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
    /// A border that also shows a drop shadow.
    /// </summary>
    public class DropShadowBorder : ContentControl
    {
        /// <summary>
        /// The drop shadow colour property.
        /// </summary>
        public static readonly DependencyProperty DropShadowColorProperty =
            DependencyProperty.Register("DropShadowColor", typeof(Color), typeof(DropShadowBorder), new PropertyMetadata(new PropertyChangedCallback(DropShadowColor_Changed)));

        /// <summary>
        /// The drop shadow opacity property.
        /// </summary>
        public static readonly DependencyProperty DropShadowOpacityProperty =
            DependencyProperty.Register("DropShadowOpacity", typeof(double), typeof(DropShadowBorder), null);

        /// <summary>
        /// The drop shadow distance property.
        /// </summary>
        public static readonly DependencyProperty DropShadowDistanceProperty =
            DependencyProperty.Register("DropShadowDistance", typeof(double), typeof(DropShadowBorder), new PropertyMetadata(new PropertyChangedCallback(DropShadowDistance_Changed)));

        /// <summary>
        /// The drop shadow angle property.
        /// </summary>
        public static readonly DependencyProperty DropShadowAngleProperty =
            DependencyProperty.Register("DropShadowAngle", typeof(double), typeof(DropShadowBorder), new PropertyMetadata(new PropertyChangedCallback(DropShadowAngle_Changed)));

        /// <summary>
        /// The drop shadow spread property.
        /// </summary>
        public static readonly DependencyProperty DropShadowSpreadProperty =
            DependencyProperty.Register("DropShadowSpread", typeof(double), typeof(DropShadowBorder), new PropertyMetadata(new PropertyChangedCallback(DropShadowSpread_Changed)));

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(DropShadowBorder), null);

        /// <summary>
        /// The shadow corner radius property.
        /// </summary>
        public static readonly DependencyProperty ShadowCornerRadiusProperty =
            DependencyProperty.Register("ShadowCornerRadius", typeof(CornerRadius), typeof(DropShadowBorder), null);

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(DropShadowBorder), null);

        /// <summary>
        /// Stores the drop shadow translate transform.
        /// </summary>
        private TranslateTransform dropShadowTranslate;

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
        /// Initialises a new instance of the <see cref="DropShadowBorder"/> class.
        /// </summary>
        public DropShadowBorder()
        {
            this.DefaultStyleKey = typeof(DropShadowBorder);
            this.SizeChanged += new SizeChangedEventHandler(this.DropShadowBorder_SizeChanged);
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
        /// Gets or sets the drop shadow colour.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("the drop shadow color.")]
        public Color DropShadowColor
        {
            get { return (Color)this.GetValue(DropShadowColorProperty); }
            set { this.SetValue(DropShadowColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow opacity.")]
        public double DropShadowOpacity
        {
            get { return (double)this.GetValue(DropShadowOpacityProperty); }
            set { this.SetValue(DropShadowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow distance.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow distance.")]
        public double DropShadowDistance
        {
            get { return (double)this.GetValue(DropShadowDistanceProperty); }
            set { this.SetValue(DropShadowDistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow angle.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow angle.")]
        public double DropShadowAngle
        {
            get { return (double)this.GetValue(DropShadowAngleProperty); }
            set { this.SetValue(DropShadowAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the drop shadow spread.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow spread.")]
        public double DropShadowSpread
        {
            get { return (double)this.GetValue(DropShadowSpreadProperty); }
            set { this.SetValue(DropShadowSpreadProperty, value); }
        }

        /// <summary>
        /// Gets the drop shadow corner radius.
        /// </summary>
        public double ShadowCornerRadius
        {
            get { return (double)this.GetValue(ShadowCornerRadiusProperty); }
        }

        /// <summary>
        /// Gets a point offset by a distance and angle (in degrees).
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <param name="distance">The distance.</param>
        /// <returns>The offset point.</returns>
        public static Point GetOffset(double angle, double distance)
        {
            double x = Math.Cos(DegreesToRadians(angle)) * distance;
            double y = Math.Tan(DegreesToRadians(angle)) * x;
            return new Point(x, y);
        }

        /// <summary>
        /// Converts degrees into radians.
        /// </summary>
        /// <param name="degrees">The degree value.</param>
        /// <returns>The degrees as radians.</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        /// <summary>
        /// Gets the parts from the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.dropShadowTranslate = (TranslateTransform)this.GetTemplateChild("PART_DropShadowTranslateTransform");
            this.shadowOuterStop1 = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop1");
            this.shadowOuterStop2 = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop2");
            this.shadowVertical1 = (GradientStop)this.GetTemplateChild("PART_ShadowVertical1");
            this.shadowVertical2 = (GradientStop)this.GetTemplateChild("PART_ShadowVertical2");
            this.shadowHorizontal1 = (GradientStop)this.GetTemplateChild("PART_ShadowHorizontal1");
            this.shadowHorizontal2 = (GradientStop)this.GetTemplateChild("PART_ShadowHorizontal2");

            this.UpdateDropShadowPosition();
            this.UpdateStops(new Size(this.ActualWidth, this.ActualHeight));
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        internal void UpdateDropShadowPosition()
        {
            if (this.dropShadowTranslate != null)
            {
                Point offset = GetOffset(this.DropShadowAngle, this.DropShadowDistance);

                this.dropShadowTranslate.X = offset.X;
                this.dropShadowTranslate.Y = offset.Y;
            }
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
                    this.shadowHorizontal1.Offset = this.DropShadowSpread / (size.Width + (this.DropShadowSpread * 2));
                }

                if (this.shadowHorizontal2 != null)
                {
                    this.shadowHorizontal2.Offset = 1 - (this.DropShadowSpread / (size.Width + (this.DropShadowSpread * 2)));
                }

                if (this.shadowVertical1 != null)
                {
                    this.shadowVertical1.Offset = this.DropShadowSpread / (size.Height + (this.DropShadowSpread * 2));
                }

                if (this.shadowVertical2 != null)
                {
                    this.shadowVertical2.Offset = 1 - (this.DropShadowSpread / (size.Height + (this.DropShadowSpread * 2)));
                }
            }
        }

        /// <summary>
        /// Updates the drop shadow colour.
        /// </summary>
        /// <param name="color">The new colour.</param>
        internal void UpdateDropShadowColor(Color color)
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
        /// Updates the drop shadow.
        /// </summary>
        /// <param name="dependencyObject">The drop shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void DropShadowDistance_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DropShadowBorder dropShadowBorder = (DropShadowBorder)dependencyObject;
            dropShadowBorder.UpdateDropShadowPosition();
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        /// <param name="dependencyObject">The drop shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void DropShadowAngle_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DropShadowBorder dropShadowBorder = (DropShadowBorder)dependencyObject;
            dropShadowBorder.UpdateDropShadowPosition();
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        /// <param name="dependencyObject">The drop shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void DropShadowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DropShadowBorder dropShadowBorder = (DropShadowBorder)dependencyObject;
            dropShadowBorder.UpdateDropShadowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the drop shadow.
        /// </summary>
        /// <param name="dependencyObject">The drop shadow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void DropShadowSpread_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DropShadowBorder dropShadowBorder = (DropShadowBorder)dependencyObject;
            dropShadowBorder.UpdateStops(new Size(dropShadowBorder.ActualWidth, dropShadowBorder.ActualHeight));
        }

        /// <summary>
        /// Updates the stops.
        /// </summary>
        /// <param name="sender">The drop shadow border.</param>
        /// <param name="e">Size changed event args.</param>
        private void DropShadowBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateStops(e.NewSize);
        }
    }
}
