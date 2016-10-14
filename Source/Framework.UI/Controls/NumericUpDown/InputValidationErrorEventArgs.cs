namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The input validation error event handler.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    public delegate void InputValidationErrorEventHandler(object sender, InputValidationErrorEventArgs e);

    /// <summary>
    /// The input validation error event args.
    /// </summary>
    public class InputValidationErrorEventArgs : EventArgs
    {
        private Exception exception;
        private bool throwException;

        /// <summary>
        /// Initialises a new instance of the <see cref="InputValidationErrorEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public InputValidationErrorEventArgs(Exception exception)
        {
            this.exception = exception;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception
        {
            get { return this.exception; }
            private set { this.exception = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether throw exception.
        /// </summary>
        public bool ThrowException
        {
            get { return this.throwException; }
            set { this.throwException = value; }
        }
    }
}
