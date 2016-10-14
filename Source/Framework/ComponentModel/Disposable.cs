namespace Framework.ComponentModel
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    /// <summary>
    /// Base class for members implementing <see cref="IDisposable"/>.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        #region Fields

        private bool isDisposed;
        private Subject<Unit> whenDisposedSubject;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Disposable"/> class.
        /// </summary>
        public Disposable()
        {
            this.whenDisposedSubject = new Subject<Unit>();
        }

        #endregion

        #region Desctructors

        /// <summary>
        /// Finalizes an instance of the <see cref="Disposable"/> class. Releases unmanaged 
        /// resources and performs other cleanup operations before the <see cref="Disposable"/> 
        /// is reclaimed by garbage collection. Will run only if the 
        /// Dispose method does not get called.
        /// </summary>
        ~Disposable()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the when errors changed observable event. Occurs when the validation errors have changed for a property or for the entire object. 
        /// </summary>
        /// <value>
        /// The when errors changed observable event.
        /// </value>
        public IObservable<Unit> WhenDisposed
        {
            get
            {
                this.ThrowIfDisposed();

                if (this.whenDisposedSubject == null)
                {
                    this.whenDisposedSubject = new Subject<Unit>();
                }

                return this.whenDisposedSubject.AsObservable();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Disposable"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        #endregion

        #region Public Methods

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
        /// Disposes the managed resources implementing <see cref="IDisposable"/>.
        /// </summary>
        protected virtual void DisposeManaged()
        {
        }

        /// <summary>
        /// Disposes the unmanaged resources implementing <see cref="IDisposable"/>.
        /// </summary>
        protected virtual void DisposeUnmanaged()
        {
        }

        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if this instance is disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
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
            // Check to see if Dispose has already been called.
            if (!this.isDisposed)
            {
                // If disposing managed and unmanaged resources.
                if (disposing)
                {
                    this.DisposeManaged();
                }

                this.DisposeUnmanaged();

                this.isDisposed = true;

                if (this.whenDisposedSubject != null)
                {
                    // Raise the WhenDisposed event.
                    this.whenDisposedSubject.OnNext(Unit.Default);
                    this.whenDisposedSubject.OnCompleted();
                    this.whenDisposedSubject.Dispose();
                }
            }
        }

        #endregion
    }
}
