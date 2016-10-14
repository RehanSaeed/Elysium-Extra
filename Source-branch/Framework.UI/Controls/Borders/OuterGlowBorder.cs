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
    /// Content control that draws and outer glow around itself.
    /// </summary>
    public class OuterGlowBorder : ContentControl
    {
        /// <summary>
        /// The outer glow opacity property.
        /// </summary>
        public static readonly DependencyProperty OuterGlowOpacityProperty =
            DependencyProperty.Register("OuterGlowOpacity", typeof(double), typeof(OuterGlowBorder), null);

        /// <summary>
        /// The outer glow size property.
        /// </summary>
        public static readonly DependencyProperty OuterGlowSizeProperty =
            DependencyProperty.Register("OuterGlowSize", typeof(double), typeof(OuterGlowBorder), null);

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(OuterGlowBorder), null);

        /// <summary>
        /// The shadow corner radius property.
        /// </summary>
        public static readonly DependencyProperty ShadowCornerRadiusProperty =
            DependencyProperty.Register("ShadowCornerRadius", typeof(CornerRadius), typeof(OuterGlowBorder), null);

        /// <summary>
        /// The outer glow colour.
        /// </summary>
        public static readonly DependencyProperty OuterGlowColorProperty =
            DependencyProperty.Register("OuterGlowColor", typeof(Color), typeof(OuterGlowBorder), new PropertyMetadata(new PropertyChangedCallback(OuterGlowColor_Changed)));

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(OuterGlowBorder), null);

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
        /// Stores the outer glow border.
        /// </summary>
        private Border outerGlowBorder;

        /// <summary>
        /// Initialises a new instance of the <see cref="OuterGlowBorder"/> class.
        /// </summary>
        public OuterGlowBorder()
        {
            this.DefaultStyleKey = typeof(OuterGlowBorder);
            this.SizeChanged += new SizeChangedEventHandler(this.OuterGlowContentControl_SizeChanged);
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
        /// Gets or sets the outer glow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow opacity.")]
        public double OuterGlowOpacity
        {
            get { return (double)this.GetValue(OuterGlowOpacityProperty); }
            set { this.SetValue(OuterGlowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the outer glow size.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow size.")]
        public double OuterGlowSize
        {
            get 
            {
                return (double)this.GetValue(OuterGlowSizeProperty); 
            }

            set 
            {
                this.SetValue(OuterGlowSizeProperty, value);
                this.UpdateGlowSize(this.OuterGlowSize);
                this.UpdateStops(new Size(this.ActualWidth, this.ActualHeight));
            }
        }

        /// <summary>
        /// Gets or sets the outer glow colour.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow color.")]
        public Color OuterGlowColor
        {
            get { return (Color)this.GetValue(OuterGlowColorProperty); }
            set { this.SetValue(OuterGlowColorProperty, value); }
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
        /// Gets the parts out of the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.shadowOuterStop1 = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop1");
            this.shadowOuterStop2 = (GradientStop)this.GetTemplateChild("PART_ShadowOuterStop2");
            this.shadowVertical1 = (GradientStop)this.GetTemplateChild("PART_ShadowVertical1");
            this.shadowVertical2 = (GradientStop)this.GetTemplateChild("PART_ShadowVertical2");
            this.shadowHorizontal1 = (GradientStop)this.GetTemplateChild("PART_ShadowHorizontal1");
            this.shadowHorizontal2 = (GradientStop)this.GetTemplateChild("PART_ShadowHorizontal2");
            this.outerGlowBorder = (Border)this.GetTemplateChild("PART_OuterGlowBorder");
            this.UpdateGlowSize(this.OuterGlowSize);
            this.UpdateGlowColor(this.OuterGlowColor);
        }

        /// <summary>
        /// Updates the glow size.
        /// </summary>
        /// <param name="size">The new size.</param>
        internal void UpdateGlowSize(double size)
        {
            if (this.outerGlowBorder != null)
            {
                this.outerGlowBorder.Margin = new Thickness(-Math.Abs(size));
            }
        }

        /// <summary>
        /// Updates the outer glow colour.
        /// </summary>
        /// <param name="color">The new colour.</param>
        internal void UpdateGlowColor(Color color)
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
        /// Updates the outer glow colour when the DP changes.
        /// </summary>
        /// <param name="dependencyObject">The outer glow border.</param>
        /// <param name="eventArgs">The new property event args.</param>
        private static void OuterGlowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            OuterGlowBorder outerGlowBorder = (OuterGlowBorder)dependencyObject;
            outerGlowBorder.UpdateGlowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the gradient stops on the drop shadow.
        /// </summary>
        /// <param name="sender">The outer glow border.</param>
        /// <param name="e">Size changed event args.</param>
        private void OuterGlowContentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateStops(e.NewSize);
        }

        /// <summary>
        /// Updates the gradient stops.
        /// </summary>
        /// <param name="size">The size of the control.</param>
        private void UpdateStops(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                if (this.shadowHorizontal1 != null)
                {
                    this.shadowHorizontal1.Offset = Math.Abs(this.OuterGlowSize) / (size.Width + Math.Abs(this.OuterGlowSize) + Math.Abs(this.OuterGlowSize));
                }

                if (this.shadowHorizontal2 != null)
                {
                    this.shadowHorizontal2.Offset = 1 - (Math.Abs(this.OuterGlowSize) / (size.Width + Math.Abs(this.OuterGlowSize) + Math.Abs(this.OuterGlowSize)));
                }

                if (this.shadowVertical1 != null)
                {
                    this.shadowVertical1.Offset = Math.Abs(this.OuterGlowSize) / (size.Height + Math.Abs(this.OuterGlowSize) + Math.Abs(this.OuterGlowSize));
                }

                if (this.shadowVertical2 != null)
                {
                    this.shadowVertical2.Offset = 1 - (Math.Abs(this.OuterGlowSize) / (size.Height + Math.Abs(this.OuterGlowSize) + Math.Abs(this.OuterGlowSize)));
                }
            }
        }
    }
}
