namespace Framework.UI.Controls
{
    /// <summary>
    /// A page with a number.
    /// </summary>
    public sealed class NumberedPage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NumberedPage"/> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        public NumberedPage(int pageNumber)
        {
            this.PageNumber = pageNumber;
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; set; }
    }
}
