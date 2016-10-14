namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// <see cref="MenuItem"/> attached properties.
    /// </summary>
    public static class MenuItemAttached
    {
        #region Dependency Properties

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached(
          "GroupName",
          typeof(string),
          typeof(MenuItemAttached),
          new PropertyMetadata(null, OnGroupNameChanged)); 

        #endregion

        private static readonly Dictionary<MenuItem, string> elementToGroupNames;

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="MenuItemAttached"/> class.
        /// </summary>
        static MenuItemAttached()
        {
            elementToGroupNames = new Dictionary<MenuItem, string>();
        } 

        #endregion

        #region Public Static Methods

        public static string GetGroupName(MenuItem menuItem)
        {
            return menuItem.GetValue(GroupNameProperty).ToString();
        }

        public static void SetGroupName(MenuItem menuItem, string value)
        {
            menuItem.SetValue(GroupNameProperty, value);
        } 

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the group name is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnGroupNameChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)dependencyObject;

            string oldGroupName = e.OldValue == null ? null : e.OldValue.ToString();
            string newGroupName = e.NewValue == null ? null : e.NewValue.ToString();

            if (string.IsNullOrEmpty(newGroupName))
            {
                RemoveMenuItemFromGroup(menuItem);
            }
            else
            {
                if (!string.Equals(newGroupName, oldGroupName, StringComparison.Ordinal))
                {
                    if (!string.IsNullOrEmpty(oldGroupName))
                    {
                        RemoveMenuItemFromGroup(menuItem);
                    }

                    AddMenuItemToGroup(menuItem, newGroupName);
                }
            }
        }

        /// <summary>
        /// Adds the menu item to the specified group.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// <param name="groupName">Name of the group.</param>
        private static void AddMenuItemToGroup(MenuItem menuItem, string groupName)
        {
            elementToGroupNames.Add(menuItem, groupName);
            menuItem.Checked += OnMenuItemChecked;
        }

        /// <summary>
        /// Removes the menu item from it's group.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        private static void RemoveMenuItemFromGroup(MenuItem menuItem)
        {
            elementToGroupNames.Remove(menuItem);
            menuItem.Checked -= OnMenuItemChecked;
        }

        /// <summary>
        /// Called when a menu item is checked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private static void OnMenuItemChecked(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.OriginalSource;

            string groupName = GetGroupName(menuItem);

            foreach (var item in elementToGroupNames
                .Where(item => !object.ReferenceEquals(item.Key, menuItem) && string.Equals(item.Value, groupName, StringComparison.Ordinal)))
            {
                item.Key.IsChecked = false;
            }
        }

        #endregion
    }
}
