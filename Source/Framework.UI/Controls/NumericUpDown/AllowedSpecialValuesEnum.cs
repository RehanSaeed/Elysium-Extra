namespace Framework.UI.Controls
{
    using System;

    /// <summary>
    /// Flagged enumeration determining which special values if any are allowed.
    /// </summary>
    [Flags]
    public enum AllowedSpecialValues
    {
        /// <summary>
        /// None allowed.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// NaN allowed.
        /// </summary>
        NaN = 1,

        /// <summary>
        /// Positive infinity allowed.
        /// </summary>
        PositiveInfinity = 2,

        /// <summary>
        /// Negative infinity allowed.
        /// </summary>
        NegativeInfinity = 4,

        /// <summary>
        /// Any infinity allowed.
        /// </summary>
        AnyInfinity = PositiveInfinity | NegativeInfinity,

        /// <summary>
        /// Any allowed.
        /// </summary>
        Any = NaN | AnyInfinity
    }
}
