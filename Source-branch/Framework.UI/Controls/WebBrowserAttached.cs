namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// <see cref="WebBrowser"/> attached properties.
    /// </summary>
    public static class WebBrowserAttached
    {
        public static readonly DependencyProperty BindableSourceProperty = DependencyProperty.RegisterAttached(
            "BindableSource", 
            typeof(string),
            typeof(WebBrowserAttached), 
            new PropertyMetadata(null, OnBindableSourcePropertyChanged));

        #region Public Static Methods

        /// <summary>
        /// Gets the bind-able version of the source property.
        /// </summary>
        /// <param name="webBrowser">The web browser.</param>
        /// <returns>The source.</returns>
        public static string GetBindableSource(WebBrowser webBrowser)
        {
            return (string)webBrowser.GetValue(BindableSourceProperty);
        }

        /// <summary>
        /// Sets the bind-able version of the source property.
        /// </summary>
        /// <param name="webBrowser">The web browser.</param>
        /// <param name="value">The source value.</param>
        public static void SetBindableSource(WebBrowser webBrowser, string value)
        {
            webBrowser.SetValue(BindableSourceProperty, value);
        } 

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the bind-able source property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnBindableSourcePropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)dependencyObject;
            string uri = e.NewValue as string;
            browser.Source = uri != null ? new Uri(uri) : null;
        } 

        #endregion
    }
}
