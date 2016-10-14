namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// The query move focus event args.
    /// </summary>
    public sealed class QueryMoveFocusEventArgs : RoutedEventArgs
    {
        // Defaults to true... if nobody does nothing, then its capable of moving focus.
        private bool canMove = true;
        private bool reachedMaxLength;
        private FocusNavigationDirection navigationDirection;

        /// <summary>
        /// Initialises a new instance of the <see cref="QueryMoveFocusEventArgs"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="reachedMaxLength">if set to <c>true</c> the max length has been reached.</param>
        internal QueryMoveFocusEventArgs(FocusNavigationDirection direction, bool reachedMaxLength)
            : base(TextBoxExtended.QueryMoveFocusEvent)
        {
            this.navigationDirection = direction;
            this.reachedMaxLength = reachedMaxLength;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="QueryMoveFocusEventArgs"/> class from being created.
        /// </summary>
        private QueryMoveFocusEventArgs()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether can move focus.
        /// </summary>
        public bool CanMoveFocus
        {
            get { return this.canMove; }
            set { this.canMove = value; }
        }

        /// <summary>
        /// Gets the focus navigation direction.
        /// </summary>
        public FocusNavigationDirection FocusNavigationDirection
        {
            get { return this.navigationDirection; }
        }

        /// <summary>
        /// Gets a value indicating whether reached max length.
        /// </summary>
        public bool ReachedMaxLength
        {
            get { return this.reachedMaxLength; }
        }
    }
}
