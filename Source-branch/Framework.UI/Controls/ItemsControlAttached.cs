namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// <see cref="ItemsControl"/> attached properties.
    /// </summary>
    public static class ItemsControlAttached
    {
        public static readonly DependencyProperty GroupStylesProperty = DependencyProperty.RegisterAttached(
            "GroupStyles",
            typeof(GroupStyleCollection), 
            typeof(ItemsControlAttached), 
            new PropertyMetadata(null, OnGroupStylesPropertyChanged));

        #region Public Methods

        /// <summary>
        /// Gets the group styles.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <returns>The group style.</returns>
        public static GroupStyleCollection GetGroupStyles(ItemsControl itemsControl)
        {
            return (GroupStyleCollection)itemsControl.GetValue(GroupStylesProperty);
        }

        /// <summary>
        /// Sets the group styles.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="value">The value.</param>
        public static void SetGroupStyles(ItemsControl itemsControl, GroupStyleCollection value)
        {
            itemsControl.SetValue(GroupStylesProperty, value);
        } 

        #endregion

        #region Private Static Properties
        
        /// <summary>
        /// Called when the group styles property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnGroupStylesPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)dependencyObject;

            GroupStyleCollection groupStyles = GetGroupStyles(itemsControl);
            if (groupStyles != null)
            {
                itemsControl.GroupStyle.Clear();
                foreach (GroupStyle groupStyle in groupStyles)
                {
                    itemsControl.GroupStyle.Add(groupStyle);
                }
            }
        } 

        #endregion
    }
}
