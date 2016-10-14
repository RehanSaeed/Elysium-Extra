namespace Framework.ComponentModel
{
    using System;
    using System.ComponentModel;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Notifies subscribers that a property in this instance is changing or has changed.
    /// </summary>
    public abstract class NotifyPropertyChanges : Disposable, INotifyPropertyChanged //, INotifyPropertyChanging
    {
        #region Public Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.propertyChanged += value; }
            remove { this.propertyChanged -= value; }
        }

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        //event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        //{
        //    add { this.PropertyChanging += value; }
        //    remove { this.PropertyChanging -= value; }
        //}

        #endregion

        #region Private Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        private event PropertyChangedEventHandler propertyChanged;

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        // private event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the when property changed observable event. Occurs when a property value changes.
        /// </summary>
        /// <value>
        /// The when property changed observable event.
        /// </value>
        public IObservable<string> WhenPropertyChanged
        {
            get
            {
                return Observable
                    .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        h => this.propertyChanged += h,
                        h => this.propertyChanged -= h)
                    .Select(x => x.EventArgs.PropertyName);
            }
        }

        /// <summary>
        /// Gets the when property changing observable event. Occurs when a property value is changing.
        /// </summary>
        /// <value>
        /// The when property changing observable event.
        /// </value>
        // public IObservable<EventPattern<PropertyChangingEventArgs>> WhenPropertyChanging
        // {
        //     get
        //     {
        //         return Observable
        //             .FromEventPattern<PropertyChangingEventHandler, PropertyChangingEventArgs>(
        //                 h => this.PropertyChanging += h,
        //                 h => this.PropertyChanging -= h)
        //             .AsObservable();
        //     }
        // }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //Debug.Assert(
            //    string.IsNullOrEmpty(propertyName) ||
            //    (this.GetType().GetProperty(propertyName) != null),
            //    "Check that the property name exists for this instance.");

            PropertyChangedEventHandler eventHandler = this.propertyChanged;

            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            if (propertyNames == null)
            {
                throw new ArgumentNullException("propertyNames");
            }

            foreach (string propertyName in propertyNames)
            {
                this.OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            //Debug.Assert(
            //    string.IsNullOrEmpty(propertyName) ||
            //    (this.GetType().GetProperty(propertyName) != null),
            //    "Check that the property name exists for this instance.");

            //PropertyChangingEventHandler eventHandler = this.PropertyChanging;

            //if (eventHandler != null)
            //{
            //    eventHandler(this, new PropertyChangingEventArgs(propertyName));
            //}
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        protected void OnPropertyChanging(params string[] propertyNames)
        {
            if (propertyNames == null)
            {
                throw new ArgumentNullException("propertyNames");
            }

            foreach (string propertyName in propertyNames)
            {
                this.OnPropertyChanging(propertyName);
            }
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="currentValue">The current value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c>.</returns>
        protected bool SetProperty<TProp>(
            ref TProp currentValue,
            TProp newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(currentValue, newValue))
            {
                this.ThrowIfDisposed();

                this.OnPropertyChanging(propertyName);
                currentValue = newValue;
                this.OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="currentValue">The current value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        /// <param name="propertyNames">The names of all properties changed.</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c>.</returns>
        protected bool SetProperty<TProp>(
            ref TProp currentValue,
            TProp newValue,
            params string[] propertyNames)
        {
            if (!object.Equals(currentValue, newValue))
            {
                this.ThrowIfDisposed();

                this.OnPropertyChanging(propertyNames);
                currentValue = newValue;
                this.OnPropertyChanged(propertyNames);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed.
        /// </summary>
        /// <param name="action">The action where the property is set.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void SetProperty(
            Action action,
            [CallerMemberName] string propertyName = null)
        {
            this.ThrowIfDisposed();

            this.OnPropertyChanging(propertyName);
            action();
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed.
        /// </summary>
        /// <param name="action">The action where the property is set.</param>
        /// <param name="propertyNames">The property names.</param>
        protected void SetProperty(
            Action action,
            params string[] propertyNames)
        {
            this.ThrowIfDisposed();

            this.OnPropertyChanging(propertyNames);
            action();
            this.OnPropertyChanged(propertyNames);
        }

        #endregion
    }
}
