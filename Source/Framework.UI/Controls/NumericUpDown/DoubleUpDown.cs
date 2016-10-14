namespace Framework.UI.Controls
{
    using System;
    using System.Windows;

    /// <summary>
    /// The double up down.
    /// </summary>
    public class DoubleUpDown : CommonNumericUpDown<double>
    {
        public static readonly DependencyProperty AllowInputSpecialValuesProperty = DependencyProperty.Register(
            "AllowInputSpecialValues",
            typeof(AllowedSpecialValues),
            typeof(DoubleUpDown),
            new UIPropertyMetadata(AllowedSpecialValues.None));

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="DoubleUpDown"/> class.
        /// </summary>
        static DoubleUpDown()
        {
            CommonNumericUpDown<double>.UpdateMetadata(typeof(DoubleUpDown), 1d, double.NegativeInfinity, double.PositiveInfinity);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DoubleUpDown"/> class.
        /// </summary>
        public DoubleUpDown()
            : base(double.Parse, decimal.ToDouble, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
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
        /// <returns> The <see cref="double?"/>. </returns>
        /// <exception cref="ArgumentException">Any Exception</exception>
        protected override double? OnCoerceIncrement(double? baseValue)
        {
            if (baseValue.HasValue && double.IsNaN(baseValue.Value))
            {
                throw new ArgumentException("NaN is invalid for Increment.");
            }

            return base.OnCoerceIncrement(baseValue);
        }

        /// <summary>
        /// The on coerce maximum.
        /// </summary>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="double?"/>. </returns>
        /// <exception cref="ArgumentException">Any Exception </exception>
        protected override double? OnCoerceMaximum(double? baseValue)
        {
            if (baseValue.HasValue && double.IsNaN(baseValue.Value))
            {
                throw new ArgumentException("NaN is invalid for Maximum.");
            }

            return base.OnCoerceMaximum(baseValue);
        }

        /// <summary>
        /// The on coerce minimum.
        /// </summary>
        /// <param name="baseValue"> The base value. </param>
        /// <returns> The <see cref="double?"/>. </returns>
        /// <exception cref="ArgumentException">Any Exception </exception>
        protected override double? OnCoerceMinimum(double? baseValue)
        {
            if (baseValue.HasValue && double.IsNaN(baseValue.Value))
            {
                throw new ArgumentException("NaN is for Minimum.");
            }

            return base.OnCoerceMinimum(baseValue);
        }

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="double"/>. </returns>
        protected override double IncrementValue(double value, double increment)
        {
            return value + increment;
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="double"/>. </returns>
        protected override double DecrementValue(double value, double increment)
        {
            return value - increment;
        }

        /// <summary>
        /// The set valid spin direction.
        /// </summary>
        protected override void SetValidSpinDirection()
        {
            if (this.Value.HasValue && double.IsInfinity(this.Value.Value) && (this.Spinner != null))
            {
                this.Spinner.ValidSpinDirection = ValidSpinDirections.None;
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
        /// <returns> The <see cref="double?"/>. </returns>
        protected override double? ConvertTextToValue(string text)
        {
            double? result = base.ConvertTextToValue(text);
            if (result != null)
            {
                if (double.IsNaN(result.Value))
                {
                    this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.NaN);
                }
                else if (double.IsPositiveInfinity(result.Value))
                {
                    this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.PositiveInfinity);
                }
                else if (double.IsNegativeInfinity(result.Value))
                {
                    this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.NegativeInfinity);
                }
            }

            return result;
        }

        #endregion
    }
}
