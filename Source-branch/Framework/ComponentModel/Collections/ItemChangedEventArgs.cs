namespace Framework.ComponentModel
{
    using System.ComponentModel;

    /// <summary>
    /// Provides data for the ItemChanged event.
    /// </summary>
    /// <typeparam name="T">The type of the item that changed.</typeparam>
    public sealed class ItemChangedEventArgs<T> : PropertyChangedEventArgs
    {
        private readonly T item;
        private readonly string propertyName;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item that changed.</param>
        /// <param name="propertyName">Name of the property that changed on the item.</param>
        public ItemChangedEventArgs(T item, string propertyName)
            : base(propertyName)
        {
            this.item = item;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the item that changed.
        /// </summary>
        /// <value>The item that changed.</value>
        public T Item
        {
            get { return this.item; }
        }

        /// <summary>
        /// Gets the name of the property that changed.
        /// </summary>
        /// <returns>The name of the property that changed.</returns>
        public string PropertyName
        {
            get { return this.propertyName; }
        }

        #endregion
    }
}
