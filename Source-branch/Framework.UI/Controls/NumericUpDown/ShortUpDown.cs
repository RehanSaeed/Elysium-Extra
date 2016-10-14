namespace Framework.UI.Controls
{
    /// <summary>
    /// The short up down.
    /// </summary>
    public class ShortUpDown : CommonNumericUpDown<short>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="ShortUpDown"/> class.
        /// </summary>
        static ShortUpDown()
        {
            CommonNumericUpDown<short>.UpdateMetadata(typeof(ShortUpDown), (short)1, short.MinValue, short.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ShortUpDown"/> class.
        /// </summary>
        public ShortUpDown()
            : base(short.Parse, decimal.ToInt16, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="short"/>. </returns>
        protected override short IncrementValue(short value, short increment)
        {
            return (short)(value + increment);
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="short"/>. </returns>
        protected override short DecrementValue(short value, short increment)
        {
            return (short)(value - increment);
        }

        #endregion //Base Class Overrides
    }
}
