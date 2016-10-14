namespace Framework.ComponentModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    /// <summary>
    /// An <see cref="ObservableCollection{T}"/> which is also capable of tracking when an item has changed.
    /// </summary>
    /// <typeparam name="T">The type of the items within this instance.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class ObservableItemsCollection<T> : ObservableCollection<T>, IDisposable
    {
        private readonly Subject<ItemChangedEventArgs<T>> whenItemChangedSubject;

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ObservableItemsCollection{T}"/> class.
        /// </summary>
        public ObservableItemsCollection()
        {
            this.whenItemChangedSubject = new Subject<ItemChangedEventArgs<T>>();
        }

        #endregion

        #region Desctructors

        /// <summary>
        /// Finalizes an instance of the <see cref="ObservableItemsCollection{T}"/> class. Releases unmanaged 
        /// resources and performs other cleanup operations before the <see cref="Disposable"/> 
        /// is reclaimed by garbage collection. Will run only if the 
        /// Dispose method does not get called.
        /// </summary>
        ~ObservableItemsCollection()
        {
            this.Dispose(false);
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
        /// Gets the when item changed observable event. Occurs when an item in the collection is changed.
        /// </summary>
        /// <value>
        /// The when item changed observable event. 
        /// </value>
        public IObservable<ItemChangedEventArgs<T>> WhenItemChanged
        {
            get { return this.whenItemChangedSubject.AsObservable(); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the elements of the specified collection to this instance.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                foreach (T item in collection)
                {
                    this.Add(item);
                }
            }
        }
        
        /// <summary>
        /// Returns a read-only <see cref="ReadOnlyObservableCollection{T}"/> wrapper for the current
        /// collection.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyObservableCollection{T}"/> that acts as a read-only
        /// wrapper around the current collection.</returns>
        public ReadOnlyObservableCollection<T> AsReadOnly()
        {
            return new ReadOnlyObservableCollection<T>(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Dispose all managed and unmanaged resources.
            this.Dispose(true);

            // Take this object off the finalization queue and prevent finalization code for this 
            // object from executing a second time.
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        protected override void ClearItems()
        {
            foreach (T item in this.Items)
            {
                this.UnregisterPropertyChangedEvent(item);
            }

            base.ClearItems();
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            this.RegisterPropertyChangedEvent(item);
        }

        /// <summary>
        /// Called when an item in the collection has changed.
        /// </summary>
        /// <param name="e">The item changed event arguments.</param>
        protected virtual void OnItemChanged(ItemChangedEventArgs<T> e)
        {
            this.whenItemChangedSubject.OnNext(e);
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            T item = this.Items[index];
            this.UnregisterPropertyChangedEvent(item);

            base.RemoveItem(index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected override void SetItem(int index, T item)
        {
            T oldItem = this.Items[index];
            this.UnregisterPropertyChangedEvent(oldItem);

            base.SetItem(index, item);

            this.RegisterPropertyChangedEvent(item);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; 
        /// <c>false</c> to release only unmanaged resources, called from the finalizer only.</param>
        private void Dispose(bool disposing)
        {
            // If disposing managed and unmanaged resources.
            if (disposing)
            {
                this.whenItemChangedSubject.Dispose();
            }
        }

        /// <summary>
        /// Registers the property changed event.
        /// </summary>
        /// <param name="item">The item.</param>
        private void RegisterPropertyChangedEvent(T item)
        {
            INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
            {
                notifyPropertyChanged.PropertyChanged -= this.OnItemPropertyChanged;
            }
        }

        /// <summary>
        /// Unregisters the property changed event.
        /// </summary>
        /// <param name="item">The item.</param>
        private void UnregisterPropertyChangedEvent(T item)
        {
            INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
            {
                notifyPropertyChanged.PropertyChanged -= this.OnItemPropertyChanged;
            }
        }

        /// <summary>
        /// Called when the property in an item in this instance changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnItemChanged(new ItemChangedEventArgs<T>((T)sender, e.PropertyName));
        }

        #endregion
    }
}
