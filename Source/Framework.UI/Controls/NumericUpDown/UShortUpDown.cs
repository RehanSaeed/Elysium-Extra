namespace Framework.UI.Controls
{
    /// <summary>
    /// The u short up down.
    /// </summary>
    internal class UShortUpDown : CommonNumericUpDown<ushort>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="UShortUpDown"/> class.
        /// </summary>
        static UShortUpDown()
        {
           CommonNumericUpDown<ushort>.UpdateMetadata(typeof(UShortUpDown), (ushort)1, ushort.MinValue, ushort.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UShortUpDown"/> class.
        /// </summary>
        public UShortUpDown()
            : base(ushort.Parse, decimal.ToUInt16, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="ushort"/>. </returns>
        protected override ushort IncrementValue(ushort value, ushort increment)
        {
            return (ushort)(value + increment);
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="ushort"/>. </returns>
        protected override ushort DecrementValue(ushort value, ushort increment)
        {
            return (ushort)(value - increment);
        }

        #endregion //Base Class Overrides
    }
}
