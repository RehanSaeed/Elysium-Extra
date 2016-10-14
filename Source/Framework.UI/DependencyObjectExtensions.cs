namespace Framework.UI.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// <see cref="DependencyObject"/> extension methods.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Finds the visual child of the specified dependency object.
        /// </summary>
        /// <typeparam name="T">The type of the child to find.</typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>
        /// The visual child of the specified dependency object.
        /// </returns>
        public static T FindVisualChild<T>(this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            return FindVisualChild<T>(dependencyObject, null);
        }

        /// <summary>
        /// Finds the visual child of the specified dependency object.
        /// </summary>
        /// <typeparam name="T">The type of the child to find.</typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="name">The name of the child. <c>null</c> if none.</param>
        /// <returns>
        /// The visual child of the specified dependency object.
        /// </returns>
        public static T FindVisualChild<T>(this DependencyObject dependencyObject, string name)
            where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                    if ((child != null) && 
                        (child is T) && 
                        (string.IsNullOrEmpty(name) || ((child is FrameworkElement) && ((FrameworkElement)child).Name == name)))
                    {
                        return (T)child;
                    }

                    T grandChild = child.FindVisualChild<T>(name);
                    if (grandChild != null)
                    {
                        return grandChild;
                    }
                }
            }

            return null;

            // if (dependencyObject is ContentControl)
            // {
            //    ContentControl contentControl = (ContentControl)dependencyObject;
            //    if (contentControl.Content is DependencyObject)
            //    {
            //        DependencyObject child = contentControl.Content as DependencyObject;
            //        if (child != null)
            //        {
            //            if ((child is T) &&
            //                (string.IsNullOrEmpty(name) ||
            //                 ((child is FrameworkElement) && ((FrameworkElement)child).Name == name)))
            //            {
            //                return (T)child;
            //            }
            //            DependencyObject grandChild = child.FindVisualChild<T>(name);
            //            if (grandChild != null)
            //            {
            //                return (T)grandChild;
            //            }
            //        }
            //    }
            // }
            // if (dependencyObject is Visual)
            // {
            //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); ++i)
            //    {
            //        DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
            //        if ((child is T) &&
            //            (string.IsNullOrEmpty(name) || ((child is FrameworkElement) && ((FrameworkElement)child).Name == name)))
            //        {
            //            return (T)child;
            //        }
            //        DependencyObject grandChild = child.FindVisualChild<T>(name);
            //        if (grandChild != null)
            //        {
            //            return (T)grandChild;
            //        }
            //    }
            // }
            // return null;
        }

        /// <summary>
        /// Finds the visual children of the specified dependency object.
        /// </summary>
        /// <typeparam name="T">The type of the children to find.</typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>
        /// The visual children of the specified dependency object.
        /// </returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            return dependencyObject.FindVisualChildren<T>(null);
        }

        /// <summary>
        /// Finds the visual children of the specified dependency object.
        /// </summary>
        /// <typeparam name="T">The type of the children to find.</typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="name">The name of the children. <c>null</c> if none.</param>
        /// <returns>
        /// The visual children of the specified dependency object.
        /// </returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject dependencyObject, string name)
            where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);

                    if ((child != null) && 
                        (child is T) &&
                        (string.IsNullOrEmpty(name) || ((child is FrameworkElement) && ((FrameworkElement)child).Name == name)))
                    {
                        yield return (T)child;
                    }

                    IEnumerable<T> grandChildren = child.FindVisualChildren<T>(name);
                    if (grandChildren != null)
                    {
                        foreach (T grandChild in grandChildren)
                        {
                            yield return grandChild;
                        }
                    }
                }
            }

            // if (dependencyObject is ContentControl)
            // {
            //    ContentControl contentControl = (ContentControl)dependencyObject;
            //    DependencyObject child = contentControl.Content as DependencyObject;
            //    if (child != null)
            //    {
            //        if ((child is T) &&
            //            (string.IsNullOrEmpty(name) || ((child is FrameworkElement) && ((FrameworkElement)child).Name == name)))
            //        {
            //            yield return (T)child;
            //        }
            //        IEnumerable<T> grandChildren = child.FindVisualChildren<T>(name);
            //        if (grandChildren != null)
            //        {
            //            foreach (T grandChild in grandChildren)
            //            {
            //                yield return grandChild;
            //            }
            //        }
            //    }
            // }
            // if (dependencyObject is Visual)
            // {
            //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); ++i)
            //    {
            //        DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
            //        if ((child is T) &&
            //            (string.IsNullOrEmpty(name) || ((child is FrameworkElement) && ((FrameworkElement)child).Name == name)))
            //        {
            //            yield return (T)child;
            //        }
            //        IEnumerable<T> grandChildren = child.FindVisualChildren<T>(name);
            //        if (grandChildren != null)
            //        {
            //            foreach (T grandChild in grandChildren)
            //            {
            //                yield return grandChild;
            //            }
            //        }
            //    }
            // }
        }

        /// <summary>
        /// Finds the visual parent of the specified dependency object.
        /// </summary>
        /// <typeparam name="T">The type of the parent to find.</typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>
        /// The visual parent of the specified dependency object.
        /// </returns>
        public static T FindVisualParent<T>(this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            return FindVisualParent<T>(dependencyObject, null);
        }

        /// <summary>
        /// Finds the visual parent of the specified dependency object.
        /// </summary>
        /// <typeparam name="T">The type of the parent to find.</typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="name">The name of the parent. <c>null</c> if none.</param>
        /// <returns>
        /// The visual parent of the specified dependency object.
        /// </returns>
        public static T FindVisualParent<T>(this DependencyObject dependencyObject, string name)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);

            if ((parent == null) && (dependencyObject is FrameworkElement))
            {
                parent = ((FrameworkElement)dependencyObject).Parent;
            }

            if (parent != null)
            {
                if ((parent is T) &&
                    (string.IsNullOrEmpty(name) || ((parent is FrameworkElement) && ((FrameworkElement)parent).Name == name)))
                {
                    return (T)parent;
                }

                T grandParent = parent.FindVisualParent<T>(name);
                if (grandParent != null)
                {
                    return grandParent;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the visual children of the specified dependency object.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The visual children of the specified dependency object.</returns>
        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject dependencyObject)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); ++i)
            {
                yield return VisualTreeHelper.GetChild(dependencyObject, i);
            }
        }

        /// <summary>
        /// Gets the logical parent of the specified dependency object.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The logical parent of the specified dependency object.</returns>
        public static DependencyObject GetLogicalParent(this DependencyObject dependencyObject)
        {
            return LogicalTreeHelper.GetParent(dependencyObject);
        }

        /// <summary>
        /// Gets the visual parent of the specified dependency object.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The visual parent of the specified dependency object.</returns>
        public static DependencyObject GetVisualParent(this DependencyObject dependencyObject)
        {
            return VisualTreeHelper.GetParent(dependencyObject);
        }
    }
}
