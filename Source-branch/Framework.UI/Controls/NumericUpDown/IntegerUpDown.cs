namespace Framework.UI.Controls
{
    using System;
    using System.Windows;

    /// <summary>
    /// The integer up down.
    /// </summary>
    public class IntegerUpDown : CommonNumericUpDown<int>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="IntegerUpDown"/> class.
        /// </summary>
        static IntegerUpDown()
        {
            CommonNumericUpDown<int>.UpdateMetadata(typeof(IntegerUpDown), 1, int.MinValue, int.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="IntegerUpDown"/> class.
        /// </summary>
        public IntegerUpDown()
            : base(int.Parse, decimal.ToInt32, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="int"/>. </returns>
        protected override int IncrementValue(int value, int increment)
        {
            return value + increment;
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="int"/>. </returns>
        protected override int DecrementValue(int value, int increment)
        {
            return value - increment;
        }

        #endregion //Base Class Overrides
    }
}
