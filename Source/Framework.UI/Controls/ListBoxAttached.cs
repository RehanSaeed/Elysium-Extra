namespace Framework.UI.Controls
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// <see cref="ListBox"/> attached properties.
    /// </summary>
    public static class ListBoxAttached
    {
        #region Dependency Properties

        public static readonly DependencyProperty BindableSelectedItemsProperty = DependencyProperty.RegisterAttached(
            "BindableSelectedItems",
            typeof(INotifyCollectionChanged),
            typeof(ListBoxAttached),
            new PropertyMetadata(null, OnBindableSelectedItemsChanged));

        private static readonly DependencyPropertyKey SuppressSelectedItemsChangePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "SuppressSelectedItemsChange",
            typeof(bool),
            typeof(ListBoxAttached),
            new PropertyMetadata(false));

        public static readonly DependencyProperty SuppressSelectedItemsChangeProperty = SuppressSelectedItemsChangePropertyKey.DependencyProperty;

        #endregion

        #region Public Static Methods

        public static INotifyCollectionChanged GetBindableSelectedItems(ListBox listBox)
        {
            return (INotifyCollectionChanged)listBox.GetValue(BindableSelectedItemsProperty);
        }

        public static void SetBindableSelectedItems(ListBox listBox, INotifyCollectionChanged value)
        {
            listBox.SetValue(BindableSelectedItemsProperty, value);
        }

        private static bool GetSuppressSelectedItemsChange(ListBox listBox)
        {
            return (bool)listBox.GetValue(SuppressSelectedItemsChangeProperty);
        }

        private static void SetSuppressSelectedItemsChange(ListBox listBox, bool value)
        {
            listBox.SetValue(SuppressSelectedItemsChangePropertyKey, value);
        }

        #endregion

        #region Private Static Methods

        private static void OnBindableSelectedItemsChanged(
            DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs e)
        {
            ListBox listBox = (ListBox)dependencyObject;
           
            INotifyCollectionChanged notifyCollectionChanged = GetBindableSelectedItems(listBox);
            if (notifyCollectionChanged != null)
            {
                listBox.SelectionChanged +=
                    (sender, e2) =>
                    {
                        if (!GetSuppressSelectedItemsChange(listBox))
                        {
                            SetSuppressSelectedItemsChange(listBox, true);

                            IList list = notifyCollectionChanged as IList;

                            if (e2.AddedItems != null)
                            {
                                foreach (object item in e2.AddedItems)
                                {
                                    list.Add(item);
                                }
                            }

                            if (e2.RemovedItems != null)
                            {
                                foreach (object item in e2.RemovedItems)
                                {
                                    list.Remove(item);
                                }
                            }

                            SetSuppressSelectedItemsChange(listBox, false);
                        }
                    };

                notifyCollectionChanged.CollectionChanged +=
                    (sender, e2) =>
                    {
                        if (!GetSuppressSelectedItemsChange(listBox))
                        {
                            SetSuppressSelectedItemsChange(listBox, true);

                            if (e2.Action == NotifyCollectionChangedAction.Reset)
                            {
                                listBox.SelectedItems.Clear();
                            }
                            else
                            {
                                if (e2.NewItems != null)
                                {
                                    foreach (object item in e2.NewItems)
                                    {
                                        listBox.SelectedItems.Add(item);
                                    }
                                }

                                if (e2.OldItems != null)
                                {
                                    foreach (object item in e2.OldItems)
                                    {
                                        listBox.SelectedItems.Remove(item);
                                    }
                                }
                            }

                            SetSuppressSelectedItemsChange(listBox, false);
                        }
                    };
            }
        }

        #endregion
    }
}
