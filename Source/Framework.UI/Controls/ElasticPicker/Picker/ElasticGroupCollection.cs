namespace Framework.UI.Controls
{
    using System.Windows;

    /// <summary>
    /// The elastic group collection.
    /// </summary>
    public class ElasticGroupCollection : FreezableCollection<ElasticGroup>
    {
        /// <summary>
        /// The create instance core.
        /// </summary>
        /// <returns> The <see cref="Freezable"/> Freeze able Object </returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ElasticGroupCollection();
        }
    }
}
