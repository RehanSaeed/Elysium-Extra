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
    /// Content control that draws a glow around its inside.
    /// </summary>
    public class InnerGlowBorder : ContentControl
    {
        /// <summary>
        /// The inner glow opacity property.
        /// </summary>
        public static readonly DependencyProperty InnerGlowOpacityProperty =
            DependencyProperty.Register("InnerGlowOpacity", typeof(double), typeof(InnerGlowBorder), null);

        /// <summary>
        /// The inner glow size property.
        /// </summary>
        public static readonly DependencyProperty InnerGlowSizeProperty =
            DependencyProperty.Register("InnerGlowSize", typeof(Thickness), typeof(InnerGlowBorder), new PropertyMetadata(new PropertyChangedCallback(InnerGlowSize_Changed)));

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(InnerGlowBorder), null);

        /// <summary>
        /// The inner glow colour.
        /// </summary>
        public static readonly DependencyProperty InnerGlowColorProperty =
            DependencyProperty.Register("InnerGlowColor", typeof(Color), typeof(InnerGlowBorder), new PropertyMetadata(new PropertyChangedCallback(InnerGlowColor_Changed)));

        /// <summary>
        /// The clip content property.
        /// </summary>
        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.Register("ClipContent", typeof(bool), typeof(InnerGlowBorder), null);

        /// <summary>
        /// The content z-index property.
        /// </summary>
        public static readonly DependencyProperty ContentZIndexProperty =
            DependencyProperty.Register("ContentZIndex", typeof(int), typeof(InnerGlowBorder), null);

        /// <summary>
        /// Stores the left glow border.
        /// </summary>
        private Border leftGlow;

        /// <summary>
        /// Stores the top glow border.
        /// </summary>
        private Border topGlow;

        /// <summary>
        /// Stores the right glow border.
        /// </summary>
        private Border rightGlow;

        /// <summary>
        /// Stores the bottom glow border.
        /// </summary>
        private Border bottomGlow;

        /// <summary>
        /// Stores the left glow stop 0;
        /// </summary>
        private GradientStop leftGlowStop0;

        /// <summary>
        /// Stores the left glow stop 1;
        /// </summary>
        private GradientStop leftGlowStop1;

        /// <summary>
        /// Stores the top glow stop 0;
        /// </summary>
        private GradientStop topGlowStop0;

        /// <summary>
        /// Stores the top glow stop 1;
        /// </summary>
        private GradientStop topGlowStop1;

        /// <summary>
        /// Stores the right glow stop 0;
        /// </summary>
        private GradientStop rightGlowStop0;

        /// <summary>
        /// Stores the right glow stop 1.
        /// </summary>
        private GradientStop rightGlowStop1;

        /// <summary>
        /// Stores the bottom glow stop 0;
        /// </summary>
        private GradientStop bottomGlowStop0;

        /// <summary>
        /// Stores the bottom glow stop 1.
        /// </summary>
        private GradientStop bottomGlowStop1;

        /// <summary>
        /// Initialises a new instance of the <see cref="InnerGlowBorder"/> class.
        /// </summary>
        public InnerGlowBorder()
        {
            this.DefaultStyleKey = typeof(InnerGlowBorder);
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
        /// Gets or sets the content z-index. 0 for behind shadow, 1 for in-front.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Set 0 for behind the shadow, 1 for in front.")]
        public int ContentZIndex
        {
            get { return (int)this.GetValue(ContentZIndexProperty); }
            set { this.SetValue(ContentZIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inner glow opacity.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow opacity.")]
        public double InnerGlowOpacity
        {
            get { return (double)this.GetValue(InnerGlowOpacityProperty); }
            set { this.SetValue(InnerGlowOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inner glow colour.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow color.")]
        public Color InnerGlowColor
        {
            get { return (Color)this.GetValue(InnerGlowColorProperty); }
            set { this.SetValue(InnerGlowColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inner glow size.
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow size.")]
        public Thickness InnerGlowSize
        {
            get
            {
                return (Thickness)this.GetValue(InnerGlowSizeProperty);
            }

            set
            {
                this.SetValue(InnerGlowSizeProperty, value);
                this.UpdateGlowSize(value);
            }
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
        /// Gets the template parts out.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.leftGlow = this.GetTemplateChild("PART_LeftGlow") as Border;
            this.topGlow = this.GetTemplateChild("PART_TopGlow") as Border;
            this.rightGlow = this.GetTemplateChild("PART_RightGlow") as Border;
            this.bottomGlow = this.GetTemplateChild("PART_BottomGlow") as Border;

            this.leftGlowStop0 = this.GetTemplateChild("PART_LeftGlowStop0") as GradientStop;
            this.leftGlowStop1 = this.GetTemplateChild("PART_LeftGlowStop1") as GradientStop;
            this.topGlowStop0 = this.GetTemplateChild("PART_TopGlowStop0") as GradientStop;
            this.topGlowStop1 = this.GetTemplateChild("PART_TopGlowStop1") as GradientStop;
            this.rightGlowStop0 = this.GetTemplateChild("PART_RightGlowStop0") as GradientStop;
            this.rightGlowStop1 = this.GetTemplateChild("PART_RightGlowStop1") as GradientStop;
            this.bottomGlowStop0 = this.GetTemplateChild("PART_BottomGlowStop0") as GradientStop;
            this.bottomGlowStop1 = this.GetTemplateChild("PART_BottomGlowStop1") as GradientStop;

            this.UpdateGlowColor(this.InnerGlowColor);
            this.UpdateGlowSize(this.InnerGlowSize);
        }

        /// <summary>
        /// Updates the inner glow colour.
        /// </summary>
        /// <param name="color">The new colour.</param>
        internal void UpdateGlowColor(Color color)
        {
            if (this.leftGlowStop0 != null)
            {
                this.leftGlowStop0.Color = color;
            }

            if (this.leftGlowStop1 != null)
            {
                this.leftGlowStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }

            if (this.topGlowStop0 != null)
            {
                this.topGlowStop0.Color = color;
            }

            if (this.topGlowStop1 != null)
            {
                this.topGlowStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }

            if (this.rightGlowStop0 != null)
            {
                this.rightGlowStop0.Color = color;
            }

            if (this.rightGlowStop1 != null)
            {
                this.rightGlowStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }

            if (this.bottomGlowStop0 != null)
            {
                this.bottomGlowStop0.Color = color;
            }

            if (this.bottomGlowStop1 != null)
            {
                this.bottomGlowStop1.Color = Color.FromArgb(
                    0,
                    color.R,
                    color.G,
                    color.B);
            }
        }

        /// <summary>
        /// Sets the glow size.
        /// </summary>
        /// <param name="newGlowSize">The new glow size.</param>
        internal void UpdateGlowSize(Thickness newGlowSize)
        {
            if (this.leftGlow != null)
            {
                this.leftGlow.Width = Math.Abs(newGlowSize.Left);
            }

            if (this.topGlow != null)
            {
                this.topGlow.Height = Math.Abs(newGlowSize.Top);
            }

            if (this.rightGlow != null)
            {
                this.rightGlow.Width = Math.Abs(newGlowSize.Right);
            }

            if (this.bottomGlow != null)
            {
                this.bottomGlow.Height = Math.Abs(newGlowSize.Bottom);
            }
        }

        /// <summary>
        /// Updates the inner glow colour when the DP changes.
        /// </summary>
        /// <param name="dependencyObject">The inner glow border.</param>
        /// <param name="eventArgs">The new property event args.</param>
        private static void InnerGlowColor_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            InnerGlowBorder innerGlowBorder = (InnerGlowBorder)dependencyObject;
            innerGlowBorder.UpdateGlowColor((Color)eventArgs.NewValue);
        }

        /// <summary>
        /// Updates the glow size.
        /// </summary>
        /// <param name="dependencyObject">The inner glow border.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args</param>
        private static void InnerGlowSize_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            InnerGlowBorder innerGlowBorder = (InnerGlowBorder)dependencyObject;
            innerGlowBorder.UpdateGlowSize((Thickness)eventArgs.NewValue);
        }
    }
}
