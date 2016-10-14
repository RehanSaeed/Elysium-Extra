namespace Framework.UI.Controls
{
    using System;
    using Framework.ComponentModel;

    /// <summary>
    /// The entity collection.
    /// </summary>
    public class EntityCollection : KeyedObservableItemsCollection<object, Entity>
    {
        #region Public Properties

        /// <summary>
        /// Occurs when the selection has changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        protected override void ClearItems()
        {
            foreach (Entity item in this.Items)
            {
                item.IsSelectedChanged -= this.OnSelectionChanged;
            }

            base.ClearItems();
        }

        /// <summary>
        /// Gets the key for item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The key for item.</returns>
        protected override object GetKeyForItem(Entity item)
        {
            return item.Content;
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, Entity item)
        {
            base.InsertItem(index, item);

            item.IsSelectedChanged += this.OnSelectionChanged;
        }

        /// <summary>
        /// Called when the selection changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSelectionChanged(object sender, EventArgs e)
        {
            EventHandler eventHandler = this.SelectionChanged;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            this.Items[index].IsSelectedChanged -= this.OnSelectionChanged;

            base.RemoveItem(index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected override void SetItem(int index, Entity item)
        {
            this.Items[index].IsSelectedChanged -= this.OnSelectionChanged;

            base.SetItem(index, item);

            item.IsSelectedChanged += this.OnSelectionChanged;
        }

        #endregion
    }
}
