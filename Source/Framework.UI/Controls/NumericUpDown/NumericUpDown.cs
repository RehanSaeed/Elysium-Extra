namespace Framework.UI.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// The numeric up down.
    /// </summary>
    /// <typeparam name="T">The Object</typeparam>
    public abstract class NumericUpDown<T> : UpDownBase<T>
    {
        #region Dependency Properties

        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register(
            "FormatString",
            typeof(string),
            typeof(NumericUpDown<T>),
            new UIPropertyMetadata("N0", OnFormatStringChanged));

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment",
            typeof(T),
            typeof(NumericUpDown<T>),
            new PropertyMetadata(default(T), OnIncrementChanged, OnCoerceIncrement));

        private static readonly DependencyPropertyKey IsNegativePropertyKey = DependencyProperty.RegisterReadOnly(
            "IsNegative",
            typeof(bool),
            typeof(UpDownBase<T>),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsNegativeProperty = IsNegativePropertyKey.DependencyProperty;

        public static readonly DependencyProperty IsRedOnNegativeProperty = DependencyProperty.Register(
            "IsRedOnNegative", 
            typeof(bool), 
            typeof(NumericUpDown<T>), 
            new PropertyMetadata(true));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(T),
            typeof(NumericUpDown<T>),
            new UIPropertyMetadata(default(T), OnMaximumChanged, OnCoerceMaximum));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(T),
            typeof(NumericUpDown<T>),
            new UIPropertyMetadata(default(T), OnMinimumChanged, OnCoerceMinimum));

        public static readonly DependencyProperty SelectAllOnGotFocusProperty = DependencyProperty.Register(
            "SelectAllOnGotFocus",
            typeof(bool),
            typeof(NumericUpDown<T>),
            new PropertyMetadata(true));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        public string FormatString
        {
            get { return (string)this.GetValue(FormatStringProperty); }
            set { this.SetValue(FormatStringProperty, value); }
        }

        /// <summary>
        /// Gets or sets the increment.
        /// </summary>
        public T Increment
        {
            get { return (T)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the value is negative.
        /// </summary>
        /// <value>
        /// <c>true</c> if the value is negative; otherwise, <c>false</c>.
        /// </value>
        public bool IsNegative
        {
            get { return (bool)this.GetValue(IsNegativeProperty); }
            protected set { this.SetValue(IsNegativePropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text is configuration red if the value is negative.
        /// </summary>
        /// <value>
        /// <c>true</c> if the text is red; otherwise, <c>false</c>.
        /// </value>
        public bool IsRedOnNegative
        {
            get { return (bool)this.GetValue(IsRedOnNegativeProperty); }
            set { this.SetValue(IsRedOnNegativeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public T Maximum
        {
            get { return (T)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        public T Minimum
        {
            get { return (T)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether select all on got focus.
        /// </summary>
        public bool SelectAllOnGotFocus
        {
            get { return (bool)this.GetValue(SelectAllOnGotFocusProperty); }
            set { this.SetValue(SelectAllOnGotFocusProperty, value); }
        }

        #endregion

        #region Protected Static Methods

        /// <summary>
        /// Parses the percentage from the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns>The <see cref="decimal"/>.</returns>
        protected static decimal ParsePercent(string text, IFormatProvider cultureInfo)
        {
            NumberFormatInfo info = NumberFormatInfo.GetInstance(cultureInfo);

            text = text.Replace(info.PercentSymbol, null);

            decimal result = decimal.Parse(text, NumberStyles.Any, info);
            result = result / 100;

            return result;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The on coerce increment.
        /// </summary>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="T"/>. </returns>
        protected virtual T OnCoerceIncrement(T baseValue)
        {
            return baseValue;
        }

        /// <summary>
        /// The on coerce maximum.
        /// </summary>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="T"/>. </returns>
        protected virtual T OnCoerceMaximum(T baseValue)
        {
            return baseValue;
        }

        /// <summary>
        /// The on coerce minimum.
        /// </summary>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="T"/>. </returns>
        protected virtual T OnCoerceMinimum(T baseValue)
        {
            return baseValue;
        }

        /// <summary>
        /// The on format string changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnFormatStringChanged(string oldValue, string newValue)
        {
            if (this.IsInitialized)
            {
                this.SyncTextAndValueProperties(false, null);
            }
        }

        /// <summary>
        /// The on increment changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnIncrementChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                this.SetValidSpinDirection();
            }
        }

        /// <summary>
        /// The on maximum changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnMaximumChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                this.SetValidSpinDirection();
            }
        }

        /// <summary>
        /// The on minimum changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnMinimumChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                this.SetValidSpinDirection();
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The on coerce increment.
        /// </summary>
        /// <param name="d"> The d. </param>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="object"/>. </returns>
        private static object OnCoerceIncrement(DependencyObject d, object baseValue)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                return numericUpDown.OnCoerceIncrement((T)baseValue);
            }

            return baseValue;
        }

        /// <summary>
        /// The on coerce maximum.
        /// </summary>
        /// <param name="d"> The d. </param>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="object"/>. </returns>
        private static object OnCoerceMaximum(DependencyObject d, object baseValue)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                return numericUpDown.OnCoerceMaximum((T)baseValue);
            }

            return baseValue;
        }

        /// <summary>
        /// The on coerce minimum.
        /// </summary>
        /// <param name="d"> The d. </param>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="object"/>. </returns>
        private static object OnCoerceMinimum(DependencyObject d, object baseValue)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                return numericUpDown.OnCoerceMinimum((T)baseValue);
            }

            return baseValue;
        }

        /// <summary>
        /// The on format string changed.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="e"> The e. </param>
        private static void OnFormatStringChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown<T> numericUpDown = o as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnFormatStringChanged((string)e.OldValue, (string)e.NewValue);
            }
        }

        /// <summary>
        /// The on increment changed.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="e"> The e. </param>
        private static void OnIncrementChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown<T> numericUpDown = o as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnIncrementChanged((T)e.OldValue, (T)e.NewValue);
            }
        }

        /// <summary>
        /// The on maximum changed.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="e"> The e. </param>
        private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown<T> numericUpDown = o as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnMaximumChanged((T)e.OldValue, (T)e.NewValue);
            }
        }

        /// <summary>
        /// The on minimum changed.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="e"> The e. </param>
        private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown<T> numericUpDown = o as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnMinimumChanged((T)e.OldValue, (T)e.NewValue);
            }
        }

        #endregion
    }
}
