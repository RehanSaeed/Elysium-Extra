namespace Framework.UI.Controls
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows;

    /// <summary>
    /// The common numeric up down.
    /// </summary>
    /// <typeparam name="T">The Type </typeparam>
    public abstract class CommonNumericUpDown<T> : NumericUpDown<T?> where T : struct, IFormattable, IComparable<T>
    {
        #region Dependency Properties

        public static readonly DependencyProperty AutoMoveFocusProperty = DependencyProperty.Register(
            "AutoMoveFocus",
            typeof(bool),
            typeof(CommonNumericUpDown<T>),
            new UIPropertyMetadata(true));

        /// <summary>
        /// Parsing Number style property
        /// </summary>
        public static readonly DependencyProperty ParsingNumberStyleProperty = DependencyProperty.Register(
            "ParsingNumberStyle",
            typeof(NumberStyles),
            typeof(CommonNumericUpDown<T>),
            new UIPropertyMetadata(NumberStyles.Any)); 

        #endregion

        private FromText fromText;
        private FromDecimal fromDecimal;
        private Func<T, T, bool> fromLowerThan;
        private Func<T, T, bool> fromGreaterThan;
        
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CommonNumericUpDown{T}"/> class.
        /// </summary>
        /// <param name="fromText"> The from text. </param>
        /// <param name="fromDecimal"> The from decimal. </param>
        /// <param name="fromLowerThan"> The from lower than. </param>
        /// <param name="fromGreaterThan"> The from greater than. </param>
        /// <exception cref="ArgumentNullException">Any Exception </exception>
        protected CommonNumericUpDown(FromText fromText, FromDecimal fromDecimal, Func<T, T, bool> fromLowerThan, Func<T, T, bool> fromGreaterThan)
        {
            if (fromText == null)
            {
                throw new ArgumentNullException("parseMethod");
            }

            if (fromDecimal == null)
            {
                throw new ArgumentNullException("fromDecimal");
            }

            if (fromLowerThan == null)
            {
                throw new ArgumentNullException("fromLowerThan");
            }

            if (fromGreaterThan == null)
            {
                throw new ArgumentNullException("fromGreaterThan");
            }

            this.fromText = fromText;
            this.fromDecimal = fromDecimal;
            this.fromLowerThan = fromLowerThan;
            this.fromGreaterThan = fromGreaterThan;
        }

        #endregion

        #region Protected Delegates

        /// <summary>
        /// The from text.
        /// </summary>
        /// <param name="s"> The s. </param>
        /// <param name="style"> The style. </param>
        /// <param name="provider"> The provider. </param>
        /// <returns>The Object</returns>
        protected delegate T FromText(string s, NumberStyles style, IFormatProvider provider);

        /// <summary>
        /// The from decimal.
        /// </summary>
        /// <param name="d"> The d. </param>
        /// <returns>The Object</returns>
        protected delegate T FromDecimal(decimal d); 

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether auto move focus.
        /// </summary>
        public bool AutoMoveFocus
        {
            get { return (bool)this.GetValue(AutoMoveFocusProperty); }
            set { this.SetValue(AutoMoveFocusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parsing number style.
        /// </summary>
        public NumberStyles ParsingNumberStyle
        {
            get { return (NumberStyles)this.GetValue(ParsingNumberStyleProperty); }
            set { this.SetValue(ParsingNumberStyleProperty, value); }
        }

        #endregion

        /// <summary>
        /// The update metadata.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="increment"> The increment. </param>
        /// <param name="minValue"> The min value. </param>
        /// <param name="maxValue"> The max value. </param>
        protected static void UpdateMetadata(Type type, T? increment, T? minValue, T? maxValue)
        {
            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            IncrementProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
            MaximumProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(maxValue));
            MinimumProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(minValue));
        }

        /// <summary>
        /// The on increment.
        /// </summary>
        protected override void OnIncrement()
        {
            if (!this.HandleNullSpin())
            {
                T result = this.IncrementValue(this.Value.Value, this.Increment.Value);
                this.Value = this.CoerceValueMinMax(result);
            }
        }

        /// <summary>
        /// The on decrement.
        /// </summary>
        protected override void OnDecrement()
        {
            if (!this.HandleNullSpin())
            {
                T result = this.DecrementValue(this.Value.Value, this.Increment.Value);
                this.Value = this.CoerceValueMinMax(result);
            }
        }

        /// <summary>
        /// Called when the value has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnValueChanged(T? oldValue, T? newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            if (this.Value.HasValue)
            {
                this.IsNegative = this.fromLowerThan(this.Value.Value, default(T));
            }
            else
            {
                this.IsNegative = false;
            }
        }

        /// <summary>
        /// The convert text to value.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <returns> The <see cref="T?"/>. </returns>
        protected override T? ConvertTextToValue(string text)
        {
            T? result = null;

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            // Since the conversion from Value to text using a FormartString may not be parse able,
            // we verify that the already existing text is not the exact same value.
            string currentValueText = this.ConvertValueToText();
            if (object.Equals(currentValueText, text))
            {
                return this.Value;
            }

            // Don't know why someone would format a T as %, but just in case they do.
            result = (this.FormatString.Contains("P") || this.FormatString.Contains("%")) ? 
                this.fromDecimal(NumericUpDown<T?>.ParsePercent(text, this.CultureInfo)) : 
                this.fromText(text, this.ParsingNumberStyle, this.CultureInfo);

            this.ValidateDefaultMinMax(result);

            return result;
        }

        /// <summary>
        /// The convert value to text.
        /// </summary>
        /// <returns> The <see cref="string"/>. </returns>
        protected override string ConvertValueToText()
        {
            if (this.Value == null)
            {
                return string.Empty;
            }

            return this.Value.Value.ToString(this.FormatString, this.CultureInfo);
        }

        /// <summary>
        /// The set valid spin direction.
        /// </summary>
        protected override void SetValidSpinDirection()
        {
            ValidSpinDirections validDirections = ValidSpinDirections.None;

            // Null increment always prevents spin.
            if ((this.Increment != null) && !this.IsReadOnly)
            {
                if (this.IsLowerThan(this.Value, this.Maximum) || !Value.HasValue)
                {
                    validDirections = validDirections | ValidSpinDirections.Increase;
                }

                if (this.IsGreaterThan(this.Value, this.Minimum) || !this.Value.HasValue)
                {
                    validDirections = validDirections | ValidSpinDirections.Decrease;
                }
            }

            if (this.Spinner != null)
            {
                this.Spinner.ValidSpinDirection = validDirections;
            }
        }

        /// <summary>
        /// The test input special value.
        /// </summary>
        /// <param name="allowedValues"> The allowed values. </param>
        /// <param name="valueToCompare"> The value to compare. </param>
        /// <exception cref="InvalidDataException">Any Exception </exception>
        protected void TestInputSpecialValue(AllowedSpecialValues allowedValues, AllowedSpecialValues valueToCompare)
        {
            if ((allowedValues & valueToCompare) != valueToCompare)
            {
                switch (valueToCompare)
                {
                    case AllowedSpecialValues.NaN:
                        throw new InvalidDataException("Value to parse shouldn't be NaN.");
                    case AllowedSpecialValues.PositiveInfinity:
                        throw new InvalidDataException("Value to parse shouldn't be Positive Infinity.");
                    case AllowedSpecialValues.NegativeInfinity:
                        throw new InvalidDataException("Value to parse shouldn't be Negative Infinity.");
                }
            }
        }

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="T"/>. </returns>
        protected abstract T IncrementValue(T value, T increment);

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="T"/>. </returns>
        protected abstract T DecrementValue(T value, T increment);

        /// <summary>
        /// The is lower than.
        /// </summary>
        /// <param name="value1"> The value 1. </param>
        /// <param name="value2"> The value 2. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool IsLowerThan(T? value1, T? value2)
        {
            if (value1 == null || value2 == null)
            {
                return false;
            }

            return this.fromLowerThan(value1.Value, value2.Value);
        }

        /// <summary>
        /// The is greater than.
        /// </summary>
        /// <param name="value1"> The value 1. </param>
        /// <param name="value2"> The value 2. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool IsGreaterThan(T? value1, T? value2)
        {
            if (value1 == null || value2 == null)
            {
                return false;
            }

            return this.fromGreaterThan(value1.Value, value2.Value);
        }

        /// <summary>
        /// The handle null spin.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool HandleNullSpin()
        {
            if (!Value.HasValue)
            {
                T forcedValue = DefaultValue.HasValue
                  ? DefaultValue.Value
                  : default(T);

                this.Value = this.CoerceValueMinMax(forcedValue);

                return true;
            }
            else if (!Increment.HasValue)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The coerce value min max.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The <see cref="T?"/>. </returns>
        private T? CoerceValueMinMax(T value)
        {
            if (this.IsLowerThan(value, this.Minimum))
            {
                return this.Minimum;
            }
            else if (this.IsGreaterThan(value, this.Maximum))
            {
                return this.Maximum;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// The validate default min max.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <exception cref="ArgumentOutOfRangeException">Any Exception </exception>
        private void ValidateDefaultMinMax(T? value)
        {
            // DefaultValue is always accepted.
            if (object.Equals(value, this.DefaultValue))
            {
                return;
            }

            if (this.IsLowerThan(value, this.Minimum))
            {
                throw new ArgumentOutOfRangeException("Minimum", string.Format("Value must be greater than MinValue of {0}", Minimum));
            }
            else if (this.IsGreaterThan(value, this.Maximum))
            {
                throw new ArgumentOutOfRangeException("Maximum", string.Format("Value must be less than MaxValue of {0}", Maximum));
            }
        }
    }
}
