namespace Framework.UI.Controls
{
    using Framework.ComponentModel;

    /// <summary>
    /// The wizard item model collection.
    /// </summary>
    public sealed class WizardItemModelCollection : KeyedObservableItemsCollection<string, WizardItemModel>
    {
        /// <summary>
        /// The get key for item.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <returns> The <see cref="string"/>. </returns>
        protected override string GetKeyForItem(WizardItemModel item)
        {
            return item.Id;
        }
    }
}
