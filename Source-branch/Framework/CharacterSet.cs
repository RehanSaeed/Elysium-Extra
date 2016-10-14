namespace Framework
{
    /// <summary>
    /// A selection of commonly used sets of characters.
    /// </summary>
    public static class CharacterSet
    {
        #region Alphabet

        /// <summary>
        /// All letters of the alphabet (uppercase and lowercase).
        /// </summary>
        public const string AlphabetBoth = AlphabetUppercase + AlphabetLowercase;

        /// <summary>
        /// Uppercase letters of the alphabet.
        /// </summary>
        public const string AlphabetUppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Lowercase letters of the alphabet.
        /// </summary>
        public const string AlphabetLowercase = "abcdefghijklmnopqrstuvwxyz";

        #endregion

        #region Numbers

        /// <summary>
        /// Numbers from zero to nine.
        /// </summary>
        public const string Numbers = "01234556789";

        #endregion
    }
}
