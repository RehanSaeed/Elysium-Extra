namespace Framework.UI.Controls
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// <see cref="TreeView"/> attached properties.
    /// </summary>
    public static class TreeViewAttached
    {
        #region Dependency Properties
        
        public static readonly DependencyProperty IsMultiSelectModeProperty = DependencyProperty.RegisterAttached(
           "IsMultiSelectMode",
           typeof(bool),
           typeof(TreeViewAttached),
           new PropertyMetadata(false, OnIsMultiSelectModeChanged));

        private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "SelectedItems", 
            typeof(IList), 
            typeof(TreeViewAttached), 
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey SelectedTreeViewItemsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "SelectedTreeViewItems",
            typeof(ObservableCollection<TreeViewItem>),
            typeof(TreeViewAttached),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedTreeViewItemsProperty = SelectedTreeViewItemsPropertyKey.DependencyProperty;

        #endregion
        
        #region Fields

        private static readonly PropertyInfo IsSelectionChangeActiveProperty = typeof(TreeView).GetProperty(
            "IsSelectionChangeActive",
            BindingFlags.NonPublic | BindingFlags.Instance);

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets a value determining if the <see cref="TreeView"/> is in multi-select mode. 
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns><c>true</c> if the <see cref="TreeView"/> is in multi-select mode, otherwise <c>false</c>.</returns>
        public static bool GetIsMultiSelectMode(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsMultiSelectModeProperty);
        }

        /// <summary>
        /// Gets the <see cref="TreeView"/> selected items.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The selected items.</returns>
        public static IList GetSelectedItems(DependencyObject dependencyObject)
        {
            return (IList)dependencyObject.GetValue(SelectedItemsProperty);
        }

        /// <summary>
        /// Gets the <see cref="TreeView"/> selected <see cref="TreeViewItem"/>'s.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The selected <see cref="TreeViewItem"/>'s.</returns>
        public static ObservableCollection<TreeViewItem> GetSelectedTreeViewItems(DependencyObject dependencyObject)
        {
            return (ObservableCollection<TreeViewItem>)dependencyObject.GetValue(SelectedTreeViewItemsProperty);
        }

        /// <summary>
        /// Sets a value determining if the <see cref="TreeView"/> is in multi-select mode. 
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">if set to <c>true</c> the <see cref="TreeView"/> is in multi-select mode.</param>
        public static void SetIsMultiSelectMode(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsMultiSelectModeProperty, value);
        } 

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Gets the <see cref="ItemsControl"/> container from the item.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="item">The item.</param>
        /// <returns>The container for the specified item.</returns>
        private static ItemsControl GetContainerFromItem(this ItemsControl itemsControl, object item)
        {
            IEnumerable<ItemsControl> allItems = itemsControl.ItemContainerGenerator.Items
                .Select(x => itemsControl.ItemContainerGenerator.ContainerFromItem(x) as ItemsControl)
                .Where(x => x != null)
                .Traverse(
                    x =>
                    {
                        IEnumerable<ItemsControl> childItems = x.ItemContainerGenerator.Items
                            .Select(y => x.ItemContainerGenerator.ContainerFromItem(y) as ItemsControl)
                            .Where(y => y != null);

                        return childItems;
                    });

            return allItems.FirstOrDefault(x => x.DataContext == item);
        }
        
        /// <summary>
        /// Called when is multi select mode property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsMultiSelectModeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TreeView treeView = (TreeView)dependencyObject;

            // Initialise the selected items.
            if (GetSelectedItems(treeView) == null)
            {
                SetSelectedItems(treeView, new ObservableCollection<object>());
                SetSelectedTreeViewItems(treeView, new ObservableCollection<TreeViewItem>());
            }

            treeView.SelectedItemChanged -= OnTreeViewSelectedItemChanged;

            if (GetIsMultiSelectMode(treeView))
            {
                treeView.SelectedItemChanged += OnTreeViewSelectedItemChanged;
            }
        }

        /// <summary>
        /// Sets the <see cref="TreeView"/> selected items.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The selected items.</param>
        private static void SetSelectedItems(DependencyObject dependencyObject, IList value)
        {
            dependencyObject.SetValue(SelectedItemsPropertyKey, value);
        }

        /// <summary>
        /// Sets the <see cref="TreeView"/> selected <see cref="TreeViewItem"/>'s.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The selected <see cref="TreeViewItem"/>'s.</param>
        private static void SetSelectedTreeViewItems(DependencyObject dependencyObject, ObservableCollection<TreeViewItem> value)
        {
            dependencyObject.SetValue(SelectedTreeViewItemsPropertyKey, value);
        }

        /// <summary>
        /// Called when the tree view selected item is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView treeView = (TreeView)sender;

            TreeViewItem treeViewItem = (TreeViewItem)GetContainerFromItem(treeView, treeView.SelectedItem);
            if (treeViewItem == null)
            {
                return;
            }

            IList selectedItems = GetSelectedItems(treeView);
            IList selectedTreeViewItems = GetSelectedTreeViewItems(treeView);

            // Allow multiple selection when control key is pressed.
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                // Suppress selection change notification.
                // Select all selected items then restore selection change notifications.
                object isSelectionChangeActive = IsSelectionChangeActiveProperty.GetValue(treeView, null);

                IsSelectionChangeActiveProperty.SetValue(treeView, true, null);
                foreach (TreeViewItem item in selectedTreeViewItems)
                {
                    item.IsSelected = true;
                }

                IsSelectionChangeActiveProperty.SetValue(
                    treeView,
                    isSelectionChangeActive,
                    null);
            }
            else
            {
                // Deselect all selected items except the current one.
                foreach (TreeViewItem item in selectedTreeViewItems)
                {
                    item.IsSelected = item == treeViewItem;
                }

                selectedTreeViewItems.Clear();
                selectedItems.Clear();
            }

            if (!selectedTreeViewItems.Contains(treeViewItem))
            {
                selectedTreeViewItems.Add(treeViewItem);
                selectedItems.Add(treeViewItem.DataContext);
            }
            else
            {
                // deselect if already selected
                treeViewItem.IsSelected = false;
                selectedTreeViewItems.Remove(treeViewItem);
                selectedItems.Remove(treeViewItem.DataContext);
            }
        } 

        #endregion
    }
}
