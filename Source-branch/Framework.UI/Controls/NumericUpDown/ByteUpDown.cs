namespace Framework.UI.Controls
{
    using System;
    using System.Windows;

    /// <summary>
    /// Byte Up Down
    /// </summary>
    public class ByteUpDown : CommonNumericUpDown<byte>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="ByteUpDown"/> class.
        /// </summary>
        static ByteUpDown()
        {
            CommonNumericUpDown<byte>.UpdateMetadata(typeof(ByteUpDown), (byte)1, byte.MinValue, byte.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ByteUpDown"/> class.
        /// </summary>
        public ByteUpDown()
            : base(byte.Parse, decimal.ToByte, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The increment value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="byte"/>. </returns>
        protected override byte IncrementValue(byte value, byte increment)
        {
            return (byte)(value + increment);
        }

        /// <summary>
        /// The decrement value.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="increment"> The increment. </param>
        /// <returns> The <see cref="byte"/>. </returns>
        protected override byte DecrementValue(byte value, byte increment)
        {
            return (byte)(value - increment);
        }

        #endregion
    }
}
