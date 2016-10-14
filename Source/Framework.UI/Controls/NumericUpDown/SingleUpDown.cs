namespace Framework.UI.Controls
{
    using System;
    using System.Windows;

    /// <summary>
    /// The single up down.
    /// </summary>
    public class SingleUpDown : CommonNumericUpDown<float>
    {
        public static readonly DependencyProperty AllowInputSpecialValuesProperty = DependencyProperty.Register(
            "AllowInputSpecialValues", 
            typeof(AllowedSpecialValues), 
            typeof(SingleUpDown), 
            new UIPropertyMetadata(AllowedSpecialValues.None));

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="SingleUpDown"/> class.
        /// </summary>
        static SingleUpDown()
        {
            CommonNumericUpDown<float>.UpdateMetadata(typeof(SingleUpDown), 1f, float.PositiveInfinity, float.NegativeInfinity);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SingleUpDown"/> class.
        /// </summary>
        public SingleUpDown()
            : base(float.Parse, decimal.ToSingle, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the allow input special values.
        /// </summary>
        public AllowedSpecialValues AllowInputSpecialValues
        {
            get { return (AllowedSpecialValues)this.GetValue(AllowInputSpecialValuesProperty); }
            set { this.SetValue(AllowInputSpecialValuesProperty, value); }
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The on coerce increment.
        /// </summary>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="float?"/>. </returns>
        /// <exception cref="ArgumentException">Any Exception </exception>
        protected override float? OnCoerceIncrement(float? baseValue)
        {
            if (baseValue.HasValue && float.IsNaN(baseValue.Value))
            {
                throw new ArgumentException("NaN is invalid for Increment.");
            }

            return base.OnCoerceIncrement(baseValue);
        }

        /// <summary>
        /// The on coerce maximum.
        /// </summary>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="float?"/>. </returns>
        /// <exception cref="ArgumentException">Any Exception </exception>
        protected override float? OnCoerceMaximum(float? baseValue)
        {
            if (baseValue.HasValue && float.IsNaN(baseValue.Value))
            {
                throw new ArgumentException("NaN is invalid for Maximum.");
            }

            return base.OnCoerceMaximum(baseValue);
        }

        /// <summary>
        /// The on coerce minimum.
        /// </summary>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="float?"/>. </returns>
        /// <exception cref="ArgumentException">Any Exception </exception>
        protected override float? OnCoerceMinimum(float? baseValue)
        {
            if (baseValue.HasValue && float.IsNaN(baseValue.Value))
            {
                throw new ArgumentException("NaN is invalid for Minimum.");
            }

            return base.OnCoerceMinimum(baseValue);
        }

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="float"/>. </returns>
        protected override float IncrementValue(float value, float increment)
        {
            return value + increment;
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="float"/>. </returns>
        protected override float DecrementValue(float value, float increment)
        {
            return value - increment;
        }

        /// <summary>
        /// The set valid spin direction.
        /// </summary>
        protected override void SetValidSpinDirection()
        {
            if (Value.HasValue && float.IsInfinity(Value.Value))
            {
                Spinner.ValidSpinDirection = ValidSpinDirections.None;
            }
            else
            {
                base.SetValidSpinDirection();
            }
        }

        /// <summary>
        /// The convert text to value.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <returns> The <see cref="float?"/>. </returns>
        protected override float? ConvertTextToValue(string text)
        {
            float? result = base.ConvertTextToValue(text);

            if (result != null)
            {
                if (float.IsNaN(result.Value))
                {
                    this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.NaN);
                }
                else if (float.IsPositiveInfinity(result.Value))
                {
                    this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.PositiveInfinity);
                }
                else if (float.IsNegativeInfinity(result.Value))
                {
                    this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.NegativeInfinity);
                }
            }

            return result;
        }

        #endregion
    }
}
