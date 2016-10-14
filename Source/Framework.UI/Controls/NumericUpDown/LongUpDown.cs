namespace Framework.UI.Controls
{
    using System;
    using System.Windows;

    /// <summary>
    /// The long up down.
    /// </summary>
    public class LongUpDown : CommonNumericUpDown<long>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="LongUpDown"/> class.
        /// </summary>
        static LongUpDown()
        {
            CommonNumericUpDown<long>.UpdateMetadata(typeof(LongUpDown), 1L, long.MinValue, long.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LongUpDown"/> class.
        /// </summary>
        public LongUpDown()
            : base(long.Parse, decimal.ToInt64, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="long"/>. </returns>
        protected override long IncrementValue(long value, long increment)
        {
            return value + increment;
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="long"/>. </returns>
        protected override long DecrementValue(long value, long increment)
        {
            return value - increment;
        }

        #endregion //Base Class Overrides
    }
}
