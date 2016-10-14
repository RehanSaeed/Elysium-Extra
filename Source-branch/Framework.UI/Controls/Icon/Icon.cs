namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class Icon : ContentControl
    {
        #region Dependency Properties


        private static readonly DependencyPropertyKey IsOverlayVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
            "IsOverlayVisible",
            typeof(bool),
            typeof(Icon),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsOverlayVisibleProperty = IsOverlayVisiblePropertyKey.DependencyProperty;

        public static readonly DependencyProperty OverlayBackgroundProperty = DependencyProperty.Register(
            "OverlayBackground",
            typeof(Brush),
            typeof(Icon),
            new PropertyMetadata(null));

        public static readonly DependencyProperty OverlayBorderBrushProperty = DependencyProperty.Register(
            "OverlayBorderBrush",
            typeof(Brush),
            typeof(Icon),
            new PropertyMetadata(null));

        public static readonly DependencyProperty OverlayBorderThicknessProperty = DependencyProperty.Register(
            "OverlayBorderThickness",
            typeof(Thickness),
            typeof(Icon),
            new PropertyMetadata(new Thickness()));

        private static readonly DependencyPropertyKey OverlayHeightPropertyKey = DependencyProperty.RegisterReadOnly(
            "OverlayHeight",
            typeof(double),
            typeof(Icon),
            new PropertyMetadata(0D));

        public static readonly DependencyProperty OverlayHeightProperty = OverlayHeightPropertyKey.DependencyProperty;

        public static readonly DependencyProperty OverlayHorizontalAlignmentProperty = DependencyProperty.Register(
            "OverlayHorizontalAlignment",
            typeof(HorizontalAlignment),
            typeof(Icon),
            new PropertyMetadata(HorizontalAlignment.Right));

        public static readonly DependencyProperty OverlayMarginProperty = DependencyProperty.Register(
            "OverlayMargin",
            typeof(Thickness),
            typeof(Icon),
            new PropertyMetadata(new Thickness()));

        public static readonly DependencyProperty OverlayPaddingProperty = DependencyProperty.Register(
            "OverlayPadding", 
            typeof(Thickness), 
            typeof(Icon), 
            new PropertyMetadata(new Thickness()));

        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(Icon),
            new PropertyMetadata(null, OnOverlayStylePropertyChanged));

        public static readonly DependencyProperty OverlayVerticalAlignmentProperty = DependencyProperty.Register(
            "OverlayVerticalAlignment",
            typeof(VerticalAlignment),
            typeof(Icon),
            new PropertyMetadata(VerticalAlignment.Bottom));

        private static readonly DependencyPropertyKey OverlayWidthPropertyKey = DependencyProperty.RegisterReadOnly(
            "OverlayWidth",
            typeof(double),
            typeof(Icon),
            new PropertyMetadata(0D));

        public static readonly DependencyProperty OverlayWidthProperty = OverlayWidthPropertyKey.DependencyProperty;

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size",
            typeof(IconSize),
            typeof(Icon),
            new PropertyMetadata(IconSize.Small, OnSizePropertyChanged));

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch",
            typeof(Stretch),
            typeof(Icon),
            new PropertyMetadata(Stretch.Uniform));

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="Icon"/> class.
        /// </summary>
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Icon), 
                new FrameworkPropertyMetadata(typeof(Icon)));
            HeightProperty.OverrideMetadata(
                typeof(Icon), 
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnHeightPropertyChanged)));
            WidthProperty.OverrideMetadata(
                typeof(Icon),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnWidthPropertyChanged)));
        }

        #endregion

        #region Public Properties

        public bool IsOverlayVisible
        {
            get { return (bool)this.GetValue(IsOverlayVisibleProperty); }
            private set { this.SetValue(IsOverlayVisiblePropertyKey, value); }
        }

        public Brush OverlayBackground
        {
            get { return (Brush)this.GetValue(OverlayBackgroundProperty); }
            set { this.SetValue(OverlayBackgroundProperty, value); }
        }

        public Brush OverlayBorderBrush
        {
            get { return (Brush)this.GetValue(OverlayBorderBrushProperty); }
            set { this.SetValue(OverlayBorderBrushProperty, value); }
        }

        public Thickness OverlayBorderThickness
        {
            get { return (Thickness)this.GetValue(OverlayBorderThicknessProperty); }
            set { this.SetValue(OverlayBorderThicknessProperty, value); }
        }

        public double OverlayHeight
        {
            get { return (double)this.GetValue(OverlayHeightProperty); }
            private set { this.SetValue(OverlayHeightPropertyKey, value); }
        }

        public HorizontalAlignment OverlayHorizontalAlignment
        {
            get { return (HorizontalAlignment)this.GetValue(OverlayHorizontalAlignmentProperty); }
            set { this.SetValue(OverlayHorizontalAlignmentProperty, value); }
        }

        public Thickness OverlayMargin
        {
            get { return (Thickness)this.GetValue(OverlayMarginProperty); }
            set { this.SetValue(OverlayMarginProperty, value); }
        }

        public Thickness OverlayPadding
        {
            get { return (Thickness)this.GetValue(OverlayPaddingProperty); }
            set { this.SetValue(OverlayPaddingProperty, value); }
        }

        public Style OverlayStyle
        {
            get { return (Style)this.GetValue(OverlayStyleProperty); }
            set { this.SetValue(OverlayStyleProperty, value); }
        }

        public VerticalAlignment OverlayVerticalAlignment
        {
            get { return (VerticalAlignment)this.GetValue(OverlayVerticalAlignmentProperty); }
            set { this.SetValue(OverlayVerticalAlignmentProperty, value); }
        }

        public double OverlayWidth
        {
            get { return (double)this.GetValue(OverlayWidthProperty); }
            private set { this.SetValue(OverlayWidthPropertyKey, value); }
        }

        public IconSize Size
        {
            get { return (IconSize)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        public Stretch Stretch
        {
            get { return (Stretch)this.GetValue(StretchProperty); }
            set { this.SetValue(StretchProperty, value); }
        } 

        #endregion

        #region Private Static Methods

        private static void OnHeightPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Icon icon = (Icon)dependencyObject;
            icon.OverlayHeight = (icon.Height / 4D) + 4D;
        }

        private static void OnOverlayStylePropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Icon icon = (Icon)dependencyObject;
            icon.UpdateIsOverlayVisible();
        }

        private static void OnSizePropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Icon icon = (Icon)dependencyObject;
            icon.UpdateIsOverlayVisible();
        }

        private static void OnWidthPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Icon icon = (Icon)dependencyObject;
            icon.OverlayWidth = (icon.Width / 4D) + 4D;
        }

        #endregion

        #region Private Methods

        private void UpdateIsOverlayVisible()
        {
            this.IsOverlayVisible = (this.OverlayStyle != null) &&
                ((this.Size == IconSize.Medium) ||
                (this.Size == IconSize.Large) ||
                (this.Size == IconSize.VeryLarge) ||
                (this.Size == IconSize.Custom));
        }

        #endregion
    }
}
