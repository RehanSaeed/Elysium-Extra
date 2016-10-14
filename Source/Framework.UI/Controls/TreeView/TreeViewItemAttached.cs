namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// <see cref="TreeViewItem"/> attached properties.
    /// </summary>
    public static class TreeViewItemAttached
    {
        public static readonly DependencyProperty BringIntoViewOnSelectedProperty = DependencyProperty.RegisterAttached(
            "BringIntoViewOnSelected", 
            typeof(bool), 
            typeof(TreeViewItemAttached),
            new PropertyMetadata(false, OnBringIntoViewOnSelectedChanged));

        #region Public Static Methods

        /// <summary>
        /// Gets the bring into view on selected.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns><c>true</c> if bring into view when the tree view item is selected, otherwise <c>false</c>.</returns>
        public static bool GetBringIntoViewOnSelected(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(BringIntoViewOnSelectedProperty);
        }

        /// <summary>
        /// Sets the bring into view on selected.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">if set to <c>true</c> bring into view when the tree view item is selected.</param>
        public static void SetBringIntoViewOnSelected(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(BringIntoViewOnSelectedProperty, value);
        } 

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the bring into view on selected property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnBringIntoViewOnSelectedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem treeViewItem = dependencyObject as TreeViewItem;
            
            treeViewItem.Selected -= OnTreeViewItemSelected;

            if (treeViewItem != null)
            {
                treeViewItem.Selected += OnTreeViewItemSelected;
            }
        }

        /// <summary>
        /// Called when the tree view item selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private static void OnTreeViewItemSelected(object sender, RoutedEventArgs e)
        {
            TreeViewItem treeViewItem = (TreeViewItem)sender;
            treeViewItem.BringIntoView();
        }

        #endregion
    }
}
