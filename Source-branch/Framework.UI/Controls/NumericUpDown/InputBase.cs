namespace Framework.UI.Controls
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The input base.
    /// </summary>
    public abstract class InputBase : Control
    {
        #region Dependency Properties

        public static readonly DependencyProperty CultureInfoProperty = DependencyProperty.Register(
            "CultureInfo",
            typeof(CultureInfo),
            typeof(InputBase),
            new UIPropertyMetadata(CultureInfo.CurrentCulture, OnCultureInfoChanged));

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly",
            typeof(bool),
            typeof(InputBase),
            new UIPropertyMetadata(false, OnReadOnlyChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(InputBase),
            new FrameworkPropertyMetadata(
                default(string),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnTextChanged,
                null,
                false,
                UpdateSourceTrigger.LostFocus));

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment",
            typeof(TextAlignment),
            typeof(InputBase),
            new UIPropertyMetadata(TextAlignment.Left));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark",
            typeof(object),
            typeof(InputBase),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register(
            "WatermarkTemplate",
            typeof(DataTemplate),
            typeof(InputBase),
            new UIPropertyMetadata(null));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the culture info.
        /// </summary>
        public CultureInfo CultureInfo
        {
            get { return (CultureInfo)this.GetValue(CultureInfoProperty); }
            set { this.SetValue(CultureInfoProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)this.GetValue(IsReadOnlyProperty); }
            set { this.SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)this.GetValue(TextAlignmentProperty); }
            set { this.SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the watermark.
        /// </summary>
        public object Watermark
        {
            get { return (object)this.GetValue(WatermarkProperty); }
            set { this.SetValue(WatermarkProperty, value); }
        }

        /// <summary>
        /// Gets or sets the watermark template.
        /// </summary>
        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)this.GetValue(WatermarkTemplateProperty); }
            set { this.SetValue(WatermarkTemplateProperty, value); }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The on culture info changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
        {
        }

        /// <summary>
        /// The on read only changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnReadOnlyChanged(bool oldValue, bool newValue)
        {
        }

        /// <summary>
        /// The on text changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnTextChanged(string oldValue, string newValue)
        {
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The on culture info changed.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="e"> The e. </param>
        private static void OnCultureInfoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            InputBase inputBase = o as InputBase;
            if (inputBase != null)
            {
                inputBase.OnCultureInfoChanged((CultureInfo)e.OldValue, (CultureInfo)e.NewValue);
            }
        }

        /// <summary>
        /// The on read only changed.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="e"> The e. </param>
        private static void OnReadOnlyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            InputBase inputBase = o as InputBase;
            if (inputBase != null)
            {
                inputBase.OnReadOnlyChanged((bool)e.OldValue, (bool)e.NewValue);
            }
        }

        /// <summary>
        /// The on text changed.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="e"> The e. </param>
        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            InputBase inputBase = o as InputBase;
            if (inputBase != null)
            {
                inputBase.OnTextChanged((string)e.OldValue, (string)e.NewValue);
            }
        }

        #endregion
    }
}
