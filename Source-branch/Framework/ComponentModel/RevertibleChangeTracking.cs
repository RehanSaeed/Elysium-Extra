namespace Framework.ComponentModel
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides support for rolling back changes made to this objects properties. This object 
    /// automatically saves its state before it is changed. Also provides errors for the object if 
    /// it is in an invalid state.
    /// </summary>
    /// <typeparam name="T">The type of this instance.</typeparam>
    public abstract class RevertibleChangeTracking<T> : EditableObject<T>, IRevertibleChangeTracking, IEquatable<T>
        where T : RevertibleChangeTracking<T>, new()
    {
        #region Fields

        private bool isChanged;

        private bool isChangeTrackingEnabled;

        private bool isNew = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the change tracking of the object's status is enabled.
        /// </summary>
        /// <value></value>
        /// <returns><c>true</c> if change tracking ir enabled; otherwise, <c>false</c>.
        /// </returns>
        public bool IsChangeTrackingEnabled
        {
            get 
            { 
                return this.isChangeTrackingEnabled; 
            }

            set
            {
                base.OnPropertyChanging("IsChangeTrackingEnabled");
                this.isChangeTrackingEnabled = value;
                base.OnPropertyChanged("IsChangeTrackingEnabled");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object's status changed.
        /// </summary>
        /// <value></value>
        /// <returns><c>true</c> if the object’s content has changed since the last call to <see cref="M:System.ComponentModel.IChangeTracking.AcceptChanges"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool IsChanged
        {
            get
            {
                return this.isChanged;
            }

            private set
            {
                base.OnPropertyChanging("IsChanged");
                this.isChanged = value;
                base.OnPropertyChanged("IsChanged");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is new.
        /// </summary>
        /// <value><c>true</c> if this instance is new; otherwise, <c>false</c>.</value>
        public bool IsNew
        {
            get 
            { 
                return this.isNew; 
            }

            private set
            {
                this.SetProperty(ref this.isNew, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets the object’s state to unchanged by accepting the modifications.
        /// </summary>
        public virtual void AcceptChanges()
        {
            if (this.IsNew)
            {
                this.IsNew = false;
                this.IsChangeTrackingEnabled = true;
            }
            else if (this.IsChanged)
            {
                this.EndEdit();

                this.IsChanged = false;
            }
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public override void CancelEdit()
        {
            base.CancelEdit();

            this.IsChanged = false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public virtual bool Equals(T other)
        {
            return object.Equals(this, other);
        }

        /// <summary>
        /// Resets the object’s state to unchanged by rejecting the modifications.
        /// </summary>
        public virtual void RejectChanges()
        {
            this.CancelEdit();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (this.IsChangeTrackingEnabled)
            {
                if (this.Equals(this.Original))
                {
                    this.IsChanged = false;
                }
                else
                {
                    this.IsChanged = true;
                }
            }
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (this.IsChangeTrackingEnabled)
            {
                this.BeginEdit();
            }

            base.OnPropertyChanging(propertyName);
        }

        #endregion
    }
}
