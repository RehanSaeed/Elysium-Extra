namespace Framework.UI
{
    using System.Windows.Markup;

    /// <summary>
    /// XAML helper methods.
    /// </summary>
    public static class XamlHelper
    {
        /// <summary>
        /// Clones the specified object by writing it to XAML and reading the XAML into a new object.
        /// </summary>
        /// <param name="obj">The object to be cloned.</param>
        /// <returns>The cloned object.</returns>
        public static object Clone(this object obj)
        {
            string xaml = XamlWriter.Save(obj);
            return XamlReader.Parse(xaml);
        }
    }
}
