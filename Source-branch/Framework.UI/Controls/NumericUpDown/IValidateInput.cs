namespace Framework.UI.Controls
{
    /// <summary>
    /// The ValidateInput interface.
    /// </summary>
    public interface IValidateInput
    {
        /// <summary>
        /// The input validation error.
        /// </summary>
        event InputValidationErrorEventHandler InputValidationError;

        /// <summary>
        /// The commit input.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        bool CommitInput();
    }
}
