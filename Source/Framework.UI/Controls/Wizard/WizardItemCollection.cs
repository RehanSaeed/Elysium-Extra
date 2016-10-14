namespace Framework.UI.Controls
{
    using System.Collections.Generic;
    using Framework.ComponentModel;

    /// <summary>
    /// The wizard item collection.
    /// </summary>
    public sealed class WizardItemCollection : KeyedObservableCollection<string, WizardItem>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="WizardItemCollection"/> class.
        /// </summary>
        public WizardItemCollection()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WizardItemCollection"/> class.
        /// </summary>
        /// <param name="items">The wizard items.</param>
        public WizardItemCollection(IEnumerable<WizardItem> items)
        {
            foreach (WizardItem item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// The get key for item.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <returns> The <see cref="string"/>. </returns>
        protected override string GetKeyForItem(WizardItem item)
        {
            return item.Id;
        }
    }
}
