namespace Framework.ComponentModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Reactive.Linq;

    /// <summary>
    /// Provides the abstract base class for an observable collection whose keys are embedded in the values.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public abstract class KeyedObservableCollection<TKey, TItem> : ObservableCollection<TItem>
    {
        #region Fields

        private const int DefaultThreshold = 0;
        private IEqualityComparer<TKey> comparer;
        private Dictionary<TKey, TItem> dictionary;
        private int keyCount;
        private int threshold;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedObservableCollection&lt;TKey, TItem&gt;"/> class
        /// that uses the default equality comparer.
        /// </summary>
        protected KeyedObservableCollection()
            : this(null, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedObservableCollection&lt;TKey, TItem&gt;"/> class
        /// that uses the specified equality comparer.
        /// </summary>
        /// <param name="comparer">The implementation of the <see cref="IEqualityComparer{T}"/> generic 
        /// interface to use when comparing keys, or null to use the default equality comparer for the 
        /// type of the key, obtained from <see cref="IEqualityComparer{T}.Default"/>.</param>
        protected KeyedObservableCollection(IEqualityComparer<TKey> comparer)
            : this(comparer, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedObservableCollection&lt;TKey, TItem&gt;"/> class
        /// that uses the specified equality comparer.
        /// </summary>
        /// <param name="comparer">The implementation of the <see cref="IEqualityComparer{T}"/> generic
        /// interface to use when comparing keys, or null to use the default equality comparer for the
        /// type of the key, obtained from <see cref="IEqualityComparer{T}.Default"/>.</param>
        /// <param name="dictionaryCreationThreshold">The number of elements the collection can hold 
        /// without creating a lookup dictionary (0 creates the lookup dictionary when the first item 
        /// is added), or –1 to specify that a lookup dictionary is never created.</param>
        /// <exception cref="ArgumentOutOfRangeException">dictionaryCreationThreshold is less than –1.</exception>
        protected KeyedObservableCollection(
            IEqualityComparer<TKey> comparer,
            int dictionaryCreationThreshold)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TKey>.Default;
            }

            if (dictionaryCreationThreshold == -1)
            {
                dictionaryCreationThreshold = 0x7fffffff;
            }

            if (dictionaryCreationThreshold < -1)
            {
                throw new ArgumentOutOfRangeException("dictionaryCreationThreshold");
            }

            this.comparer = comparer;
            this.threshold = dictionaryCreationThreshold;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the when collection changed observable event. Occurs when the collection is changed.
        /// </summary>
        /// <value>
        /// The when collection changed observable event. 
        /// </value>
        public IObservable<NotifyCollectionChangedEventArgs> WhenCollectionChanged
        {
            get
            {
                return Observable
                    .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                        h => this.CollectionChanged += h,
                        h => this.CollectionChanged -= h)
                    .Select(x => x.EventArgs);
            }
        }

        /// <summary>
        /// Gets the generic equality comparer that is used to determine equality of
        ///  keys in the collection.
        /// </summary>
        /// <value>The implementation of the <see cref="IEqualityComparer{T}"/> generic interface 
        /// that is used to determine equality of keys in the collection.</value>
        public IEqualityComparer<TKey> Comparer
        {
            get { return this.comparer; }
        }

        /// <summary>
        /// Gets the <see cref="TItem" /> with the specified key.
        /// </summary>
        /// <value>
        /// The element with the specified key. If an element with the specified key
        /// is not found, an exception is thrown.
        /// </value>
        /// <param name="key">The key.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
        /// <exception cref="ArgumentNullException">key is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">An element with the specified key does not exist in the collection.</exception>
        public TItem this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (this.dictionary != null)
                {
                    return this.dictionary[key];
                }

                foreach (TItem local in base.Items)
                {
                    if (this.comparer.Equals(this.GetKeyForItem(local), key))
                    {
                        return local;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the internal dictionary of items.
        /// </summary>
        /// <value>The dictionary.</value>
        protected IDictionary<TKey, TItem> Dictionary
        {
            get { return this.dictionary; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the elements of the specified collection to this instance.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void AddRange(IEnumerable<TItem> collection)
        {
            if (collection != null)
            {
                foreach (TItem item in collection)
                {
                    this.Add(item);
                }
            }
        }

        /// <summary>
        /// Returns a read-only <see cref="ReadOnlyObservableCollection{TItem}"/> wrapper for the current
        /// collection.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyObservableCollection{TItem}"/> that acts as a read-only
        /// wrapper around the current collection.</returns>
        public ReadOnlyObservableCollection<TItem> AsReadOnly()
        {
            return new ReadOnlyObservableCollection<TItem>(this);
        }

        /// <summary>
        /// Determines whether this instance contains an item with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>
        /// <c>true</c> if this instance contains an item with the specified key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">key is <c>null</c>.</exception>
        public bool Contains(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (this.dictionary != null)
            {
                return this.dictionary.ContainsKey(key);
            }

            if (key != null)
            {
                foreach (TItem local in base.Items)
                {
                    if (this.comparer.Equals(this.GetKeyForItem(local), key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the item with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element was successfully removed, otherwise <c>false</c>. 
        /// Also returns false if key is not found.</returns>
        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (this.dictionary != null)
            {
                return this.dictionary.ContainsKey(key) && base.Remove(this.dictionary[key]);
            }

            if (key != null)
            {
                for (int i = 0; i < base.Items.Count; i++)
                {
                    if (this.comparer.Equals(this.GetKeyForItem(base.Items[i]), key))
                    {
                        this.RemoveItem(i);
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Changes the key associated with the specified element in the lookup dictionary.
        /// </summary>
        /// <param name="item">The element to change the key of.</param>
        /// <param name="newKey">The new key for item.</param>
        /// <exception cref="ArgumentNullException">item is null or key is null.</exception>
        /// <exception cref="ArgumentException">item is not found or key already exists.</exception>
        protected void ChangeItemKey(TItem item, TKey newKey)
        {
            if (!this.ContainsItem(item))
            {
                throw new ArgumentException("Item does not exist.", "item");
            }

            TKey keyForItem = this.GetKeyForItem(item);
            if (!this.comparer.Equals(keyForItem, newKey))
            {
                if (newKey != null)
                {
                    this.AddKey(newKey, item);
                }

                if (keyForItem != null)
                {
                    this.RemoveKey(keyForItem);
                }
            }
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();

            if (this.dictionary != null)
            {
                this.dictionary.Clear();
            }

            this.keyCount = 0;
        }

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>The key for the specified element.</returns>
        protected abstract TKey GetKeyForItem(TItem item);

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, TItem item)
        {
            TKey keyForItem = this.GetKeyForItem(item);

            if (keyForItem != null)
            {
                this.AddKey(keyForItem, item);
            }

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            TKey keyForItem = this.GetKeyForItem(base.Items[index]);

            if (keyForItem != null)
            {
                this.RemoveKey(keyForItem);
            }

            base.RemoveItem(index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected override void SetItem(int index, TItem item)
        {
            TKey keyForItem = this.GetKeyForItem(item);
            TKey x = this.GetKeyForItem(base.Items[index]);

            if (this.comparer.Equals(x, keyForItem))
            {
                if ((keyForItem != null) && (this.dictionary != null))
                {
                    this.dictionary[keyForItem] = item;
                }
            }
            else
            {
                if (keyForItem != null)
                {
                    this.AddKey(keyForItem, item);
                }

                if (x != null)
                {
                    this.RemoveKey(x);
                }
            }

            base.SetItem(index, item);
        }

        #endregion

        #region Private Methods

        private void AddKey(TKey key, TItem item)
        {
            if (this.dictionary != null)
            {
                this.dictionary.Add(key, item);
            }
            else if (this.keyCount == this.threshold)
            {
                this.CreateDictionary();
                this.dictionary.Add(key, item);
            }
            else
            {
                if (this.Contains(key))
                {
                    throw new ArgumentException(
                        string.Format("Duplicate key. Key:<{0}>.", key), "key");
                }

                this.keyCount++;
            }
        }

        private bool ContainsItem(TItem item)
        {
            TKey local;
            TItem local2;

            if ((this.dictionary == null) || ((local = this.GetKeyForItem(item)) == null))
            {
                return base.Items.Contains(item);
            }

            return this.dictionary.TryGetValue(local, out local2) && EqualityComparer<TItem>.Default.Equals(local2, item);
        }

        private void CreateDictionary()
        {
            this.dictionary = new Dictionary<TKey, TItem>(this.comparer);

            foreach (TItem local in base.Items)
            {
                TKey keyForItem = this.GetKeyForItem(local);

                if (keyForItem != null)
                {
                    this.dictionary.Add(keyForItem, local);
                }
            }
        }

        private void RemoveKey(TKey key)
        {
            if (this.dictionary != null)
            {
                this.dictionary.Remove(key);
            }
            else
            {
                this.keyCount--;
            }
        }

        #endregion
    }
}
