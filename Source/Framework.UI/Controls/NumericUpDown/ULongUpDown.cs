namespace Framework.UI.Controls
{
    using System;
    using System.Windows;

    /// <summary>
    /// The u long up down.
    /// </summary>
    internal class ULongUpDown : CommonNumericUpDown<ulong>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="ULongUpDown"/> class.
        /// </summary>
        static ULongUpDown()
        {
            CommonNumericUpDown<ulong>.UpdateMetadata(typeof(ULongUpDown), (ulong)1, ulong.MinValue, ulong.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ULongUpDown"/> class.
        /// </summary>
        public ULongUpDown()
            : base(ulong.Parse, decimal.ToUInt64, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="ulong"/>. </returns>
        protected override ulong IncrementValue(ulong value, ulong increment)
        {
            return (ulong)(value + increment);
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="ulong"/>. </returns>
        protected override ulong DecrementValue(ulong value, ulong increment)
        {
            return (ulong)(value - increment);
        }

        #endregion //Base Class Overrides
    }
}
