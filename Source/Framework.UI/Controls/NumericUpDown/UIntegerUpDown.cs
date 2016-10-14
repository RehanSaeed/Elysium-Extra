namespace Framework.UI.Controls
{
    /// <summary>
    /// The u integer up down.
    /// </summary>
    internal class UIntegerUpDown : CommonNumericUpDown<uint>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="UIntegerUpDown"/> class.
        /// </summary>
        static UIntegerUpDown()
        {
            CommonNumericUpDown<uint>.UpdateMetadata(typeof(UIntegerUpDown), (uint)1, uint.MinValue, uint.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UIntegerUpDown"/> class.
        /// </summary>
        public UIntegerUpDown()
            : base(uint.Parse, decimal.ToUInt32, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="uint"/>. </returns>
        protected override uint IncrementValue(uint value, uint increment)
        {
            return (uint)(value + increment);
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="uint"/>. </returns>
        protected override uint DecrementValue(uint value, uint increment)
        {
            return (uint)(value - increment);
        }

        #endregion //Base Class Overrides
    }
}
