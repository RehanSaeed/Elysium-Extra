namespace Framework.UI.Controls
{
    /// <summary>
    /// The decimal up down.
    /// </summary>
    public class DecimalUpDown : CommonNumericUpDown<decimal>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="DecimalUpDown"/> class.
        /// </summary>
        static DecimalUpDown()
        {
            CommonNumericUpDown<decimal>.UpdateMetadata(typeof(DecimalUpDown), 1m, decimal.MinValue, decimal.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DecimalUpDown"/> class.
        /// </summary>
        public DecimalUpDown()
            : base(decimal.Parse, (d) => d, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="decimal"/>. </returns>
        protected override decimal IncrementValue(decimal value, decimal increment)
        {
            return value + increment;
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="decimal"/>. </returns>
        protected override decimal DecrementValue(decimal value, decimal increment)
        {
            return value - increment;
        }

        #endregion //Base Class Overrides
    }
}
