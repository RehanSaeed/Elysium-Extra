namespace Framework.ComponentModel
{
    using System;
    using System.ComponentModel;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    /// <summary>
    /// Provides functionality to commit or rollback changes to an object that is used as a data
    /// source and provide errors for the object if it is in an invalid state.
    /// </summary>
    /// <typeparam name="T">The type of this instance.</typeparam>
    public abstract class EditableObject<T> : NotifyDataErrorInfo<T>, ICloneable<T>, IEditableObject
        where T : EditableObject<T>, new()
    {
        #region Fields

        private readonly Subject<Unit> beginEditingSubject;
        private readonly Subject<Unit> cancelEditingSubject;
        private readonly Subject<Unit> endEditingSubject;

        private T original; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="EditableObject{T}"/> class.
        /// </summary>
        public EditableObject()
        {
            this.beginEditingSubject = new Subject<Unit>();
            this.cancelEditingSubject = new Subject<Unit>();
            this.endEditingSubject = new Subject<Unit>();
        } 

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the when begin editing observable event. Occurs when beginning editing.
        /// </summary>
        /// <value>
        /// The when begin editing observable event.
        /// </value>
        public IObservable<Unit> WhenBeginEditing
        {
            get { return this.beginEditingSubject.AsObservable(); }
        }

        /// <summary>
        /// Gets the when cancel editing observable event. Occurs when cancelling editing.
        /// </summary>
        /// <value>
        /// The when begin cancel observable event.
        /// </value>
        public IObservable<Unit> WhenCancelEditing
        {
            get { return this.cancelEditingSubject.AsObservable(); }
        }

        /// <summary>
        /// Gets the when end editing observable event. Occurs when ending editing.
        /// </summary>
        /// <value>
        /// The when begin end observable event.
        /// </value>
        public IObservable<Unit> WhenEndEditing
        {
            get { return this.endEditingSubject.AsObservable(); }
        }

        /// <summary>
        /// Gets the original version of this object before <see cref="BeginEdit"/> was called.
        /// </summary>
        /// <value>The original version of this object before <see cref="BeginEdit"/> was called.</value>
        public T Original
        {
            get { return this.original; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones the clonable object of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// The cloned object of type <typeparamref name="T"/>.
        /// </returns>
        public T Clone()
        {
            T clone = new T();

            clone.Load((T)this);

            return clone;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        //object ICloneable.Clone()
        //{
        //    return this.Clone();
        //}

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public virtual void BeginEdit()
        {
            if (this.original == null)
            {
                this.original = this.Clone();

                this.OnBeginEditing();
            }
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public virtual void CancelEdit()
        {
            if (this.original != null)
            {
                this.Load(this.original);

                this.original = null;

                this.OnCancelEditing();
            }
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public virtual void EndEdit()
        {
            this.original = null;

            this.OnEndEditing();
        }

        /// <summary>
        /// Loads the specified item.
        /// </summary>
        /// <param name="item">The item to load this instance from.</param>
        public abstract void Load(T item);

        #endregion

        #region Protected Methods

        /// <summary>
        /// Disposes the managed resources implementing <see cref="IDisposable" />.
        /// </summary>
        protected override void DisposeManaged()
        {
            this.beginEditingSubject.Dispose();
            this.cancelEditingSubject.Dispose();
            this.endEditingSubject.Dispose();
        }

        /// <summary>
        /// Called when editing has began.
        /// </summary>
        protected virtual void OnBeginEditing()
        {
            this.beginEditingSubject.OnNext(Unit.Default);
        }

        /// <summary>
        /// Called when editing is cancelled.
        /// </summary>
        protected virtual void OnCancelEditing()
        {
            this.cancelEditingSubject.OnNext(Unit.Default);
        }

        /// <summary>
        /// Called when editing has ended.
        /// </summary>
        protected virtual void OnEndEditing()
        {
            this.endEditingSubject.OnNext(Unit.Default);
        }

        #endregion
    }
}
