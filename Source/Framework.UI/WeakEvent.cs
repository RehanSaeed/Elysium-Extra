namespace Framework.UI.Controls
{
    using System;
    using System.Windows;

    /// <summary>
    /// Weakly registers for events using <see cref="WeakReference"/>.
    /// </summary>
    public sealed class WeakEvent
    {
        private Action removeEventHandler;

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakEvent"/> class.
        /// </summary>
        /// <param name="removeEventHandler">The remove event handler function.</param>
        private WeakEvent(Action removeEventHandler)
        {
            this.removeEventHandler = removeEventHandler;
        }

        /// <summary>
        /// Gets a value indicating whether this event is registered.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </value>
        public bool IsRegistered
        {
            get { return this.removeEventHandler != null; }
        }

        /// <summary>
        /// Weakly registers the specified subscriber to the the given event.
        /// </summary>
        /// <typeparam name="S">The type of the subscriber.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="addEventHandler">The add event handler function.</param>
        /// <param name="removeEventHandler">The remove event handler function.</param>
        /// <param name="action">The event execution function.</param>
        /// <example>
        /// TextBox textbox;
        /// WeakEvent.Register&lt;TextBox, TextChangedEventHandler, TextChangedEventArgs&gt;(
        ///     this,
        ///     eventHandler =&gt; (sender, e) =&gt; eventHandler(sender, e),
        ///     eventHandler =&gt; textBox.TextChanged += eventHandler,
        ///     eventHandler =&gt; textBox.TextChanged -= eventHandler,
        ///     (sender, e) =&gt; this.OnTextChanged(sender, e));
        /// </example>
        /// <returns>An instance of the weak event.</returns>
        public static WeakEvent Register<S>(
            S subscriber,
            Action<DependencyPropertyChangedEventHandler> addEventHandler,
            Action<DependencyPropertyChangedEventHandler> removeEventHandler,
            Action<S, DependencyPropertyChangedEventArgs> action)
            where S : class
        {
            WeakReference weakReference = new WeakReference(subscriber);

            DependencyPropertyChangedEventHandler eventHandler = null;
            eventHandler = new DependencyPropertyChangedEventHandler(
                (sender, e) =>
                {
                    S subscriberStrongRef = weakReference.Target as S;

                    if (subscriberStrongRef != null)
                    {
                        action(subscriberStrongRef, e);
                    }
                    else
                    {
                        removeEventHandler(eventHandler);
                        eventHandler = null;
                    }
                });

            addEventHandler(eventHandler);

            return new WeakEvent(() => removeEventHandler(eventHandler));
        }

        /// <summary>
        /// Weakly registers the specified subscriber to the the given event of type
        /// <see cref="EventHandler"/>.
        /// </summary>
        /// <typeparam name="S">The type of the subscriber.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="addEventhandler">The add event handler.</param>
        /// <param name="removeEventHandler">The remove event handler function.</param>
        /// <param name="action">The event execution function.</param>
        /// <example>
        /// Application application;
        /// WeakEvent.Register&lt;TextBox, TextChangedEventArgs&gt;(
        ///     this,
        ///     eventHandler =&gt; textBox.TextChanged += eventHandler,
        ///     eventHandler =&gt; textBox.TextChanged -= eventHandler,
        ///     (sender, e) =&gt; this.OnTextChanged(sender, e));
        /// </example>
        /// <returns>An instance of the weak event.</returns>
        public static WeakEvent Register<S>(
            S subscriber,
            Action<EventHandler> addEventhandler,
            Action<EventHandler> removeEventHandler,
            Action<S, EventArgs> action)
            where S : class
        {
            return Register<S, EventHandler, EventArgs>(
                subscriber,
                eventHandler => (sender, e) => eventHandler(sender, e),
                addEventhandler,
                removeEventHandler,
                action);
        }

        /// <summary>
        /// Weakly registers the specified subscriber to the the given event of type
        /// <see cref="EventHandler{T}"/>.
        /// </summary>
        /// <typeparam name="S">The type of the subscriber.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="addEventhandler">The add event handler.</param>
        /// <param name="removeEventHandler">The remove event handler function.</param>
        /// <param name="action">The event execution function.</param>
        /// <example>
        /// Application application;
        /// WeakEvent.Register&lt;TextBox, TextChangedEventArgs&gt;(
        ///     this,
        ///     eventHandler =&gt; textBox.TextChanged += eventHandler,
        ///     eventHandler =&gt; textBox.TextChanged -= eventHandler,
        ///     (sender, e) =&gt; this.OnTextChanged(sender, e));
        /// </example>
        /// <returns>An instance of the weak event.</returns>
        public static WeakEvent Register<S, TEventArgs>(
            S subscriber, 
            Action<EventHandler<TEventArgs>> addEventhandler, 
            Action<EventHandler<TEventArgs>> removeEventHandler,
            Action<S, TEventArgs> action)
            where S : class
            where TEventArgs : EventArgs
        {
            return Register<S, EventHandler<TEventArgs>, TEventArgs>(
                subscriber,
                eventHandler => eventHandler, 
                addEventhandler, 
                removeEventHandler, 
                action);
        }

        /// <summary>
        /// Weakly registers the specified subscriber to the the given event.
        /// </summary>
        /// <typeparam name="S">The type of the subscriber.</typeparam>
        /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="getEventHandler">The get event handler function.</param>
        /// <param name="addEventHandler">The add event handler function.</param>
        /// <param name="removeEventHandler">The remove event handler function.</param>
        /// <param name="action">The event execution function.</param>
        /// <example>
        /// TextBox textbox;
        /// WeakEvent.Register&lt;TextBox, TextChangedEventHandler, TextChangedEventArgs&gt;(
        ///     this,
        ///     eventHandler =&gt; (sender, e) =&gt; eventHandler(sender, e),
        ///     eventHandler =&gt; textBox.TextChanged += eventHandler,
        ///     eventHandler =&gt; textBox.TextChanged -= eventHandler,
        ///     (sender, e) =&gt; this.OnTextChanged(sender, e));
        /// </example>
        /// <returns>An instance of the weak event.</returns>
        public static WeakEvent Register<S, TEventHandler, TEventArgs>(
            S subscriber, 
            Func<EventHandler<TEventArgs>, TEventHandler> getEventHandler,
            Action<TEventHandler> addEventHandler, 
            Action<TEventHandler> removeEventHandler,
            Action<S, TEventArgs> action)
            where S : class
            where TEventHandler : class
            where TEventArgs : EventArgs
        {
            WeakReference weakReference = new WeakReference(subscriber);

            TEventHandler eventHandler = null;
            eventHandler = getEventHandler(
                new EventHandler<TEventArgs>(
                    (sender, e) =>
                    {
                        S subscriberStrongRef = weakReference.Target as S;

                        if (subscriberStrongRef != null)
                        {
                            action(subscriberStrongRef, e);
                        }
                        else
                        {
                            removeEventHandler(eventHandler);
                            eventHandler = null;
                        }
                    }));

            addEventHandler(eventHandler);

            return new WeakEvent(() => removeEventHandler(eventHandler));
        }

        /// <summary>
        /// Manually unregisters this instance from the event.
        /// </summary>
        public void Unregister()
        {
            if (this.removeEventHandler != null)
            {
                this.removeEventHandler();
                this.removeEventHandler = null;
            }
        }
    }
}
