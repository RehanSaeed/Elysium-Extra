namespace Framework.UI.Controls
{
    /// <summary>
    /// The s byte up down.
    /// </summary>
    internal class SByteUpDown : CommonNumericUpDown<sbyte>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="SByteUpDown"/> class.
        /// </summary>
        static SByteUpDown()
        {
            CommonNumericUpDown<sbyte>.UpdateMetadata(typeof(SByteUpDown), (sbyte)1, sbyte.MinValue, sbyte.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SByteUpDown"/> class.
        /// </summary>
        public SByteUpDown()
            : base(sbyte.Parse, decimal.ToSByte, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="sbyte"/>. </returns>
        protected override sbyte IncrementValue(sbyte value, sbyte increment)
        {
            return (sbyte)(value + increment);
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="sbyte"/>. </returns>
        protected override sbyte DecrementValue(sbyte value, sbyte increment)
        {
            return (sbyte)(value - increment);
        }

        #endregion //Base Class Overrides
    }
}
