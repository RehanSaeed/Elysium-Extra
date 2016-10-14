namespace Framework.UI
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// visual tree helpers.
    /// </summary>
    public class VisualTreeHelpers
    {
        /// <summary>
        /// Returns the first ancestor of specified type
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to find
        /// </typeparam>
        /// <param name="current">
        /// The current.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Returns a specific ancestor of an object
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to find.
        /// </typeparam>
        /// <param name="current">
        /// The current.
        /// </param>
        /// <param name="lookupItem">
        /// The lookup Item.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T FindAncestor<T>(DependencyObject current, T lookupItem)
            where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T && current == lookupItem)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Finds an ancestor object by name and type
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to find.
        /// </typeparam>
        /// <param name="current">
        /// The current.
        /// </param>
        /// <param name="parentName">
        /// The parent Name.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T FindAncestor<T>(DependencyObject current, string parentName)
            where T : DependencyObject
        {
            while (current != null)
            {
                if (!string.IsNullOrEmpty(parentName))
                {
                    var frameworkElement = current as FrameworkElement;
                    if (current is T && frameworkElement != null && frameworkElement.Name == parentName)
                    {
                        return (T)current;
                    }
                }
                else if (current is T)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Looks for a child control within a parent by name
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to find.
        /// </typeparam>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="childName">
        /// The child Name.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
            where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                    else
                    {
                        // recursively drill down the tree
                        foundChild = FindChild<T>(child, childName);

                        // If the child is found, break so we do not overwrite the found child.
                        if (foundChild != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Looks for a child control within a parent by type
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to find.
        /// </typeparam>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T FindChild<T>(DependencyObject parent)
            where T : DependencyObject
        {
            // Confirm parent is valid.
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                // If the child is not of the request child type child
                var childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}