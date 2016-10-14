namespace Framework.UI.Commands
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Defines a Command Binding
    /// This inherits from <see cref="Freezable"/> so that it gets inheritance context for DataBinding to work
    /// </summary>
    public class CommandBinding : Freezable, IDisposable
    {
        #region Dependency Properties

        /// <summary>
        /// Action Dependency Property
        /// </summary>
        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register(
            "Action",
            typeof(Action<object>),
            typeof(CommandBinding),
            new FrameworkPropertyMetadata(
                (Action<object>)null,
                new PropertyChangedCallback(OnActionChanged)));

        /// <summary>
        /// Command Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(CommandBinding),
            new FrameworkPropertyMetadata(
                (ICommand)null,
                new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// CommandParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(CommandBinding),
            new FrameworkPropertyMetadata(
                (object)null,
                new PropertyChangedCallback(OnCommandParameterChanged)));

        /// <summary>
        /// Event Dependency Property
        /// </summary>
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register(
            "Event",
            typeof(string),
            typeof(CommandBinding),
            new FrameworkPropertyMetadata(
                (string)null,
                new PropertyChangedCallback(OnEventChanged))); 

        #endregion

        private CommandBehaviorBinding behavior;
        private bool isDisposed;
        private DependencyObject owner;

        /// <summary>
        /// Finalises an instance of the <see cref="CommandBinding"/> class. Releases unmanaged 
        /// resources and performs other clean-up operations before the <see cref="CommandBinding"/> 
        /// is reclaimed by garbage collection. Will run only if the 
        /// Dispose method does not get called.
        /// </summary>
        ~CommandBinding()
        {
            this.Dispose(false);
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the Action property. 
        /// </summary>
        public Action<object> Action
        {
            get { return (Action<object>)this.GetValue(ActionProperty); }
            set { this.SetValue(ActionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Command property.  
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the CommandParameter property.  
        /// </summary>
        public object CommandParameter
        {
            get { return (object)this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Event property.  
        /// </summary>
        public string Event
        {
            get { return (string)this.GetValue(EventProperty); }
            set { this.SetValue(EventProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner
        {
            get
            {
                return this.owner;
            }

            set
            {
                this.owner = value;
                this.ResetEventBinding();
            }
        } 

        #endregion

        /// <summary>
        /// Gets the command behaviour binding.
        /// </summary>
        internal CommandBehaviorBinding Behavior
        {
            get
            {
                if (this.behavior == null)
                {
                    this.behavior = new CommandBehaviorBinding();
                }

                return this.behavior;
            }
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

        #region Protected Methods

        /// <summary>
        /// This is not actually used. This is just a trick so that this object gets WPF Inheritance Context.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Action property.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnActionChanged(DependencyPropertyChangedEventArgs e)
        {
            this.Behavior.Action = this.Action;
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Command property.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            this.Behavior.Command = this.Command;
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CommandParameter property.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCommandParameterChanged(DependencyPropertyChangedEventArgs e)
        {
            this.Behavior.CommandParameter = this.CommandParameter;
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Event property.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnEventChanged(DependencyPropertyChangedEventArgs e)
        {
            this.ResetEventBinding();
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Handles changes to the Action property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CommandBinding)d).OnActionChanged(e);
        }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CommandBinding)d).OnCommandChanged(e);
        }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CommandBinding)d).OnCommandParameterChanged(e);
        }

        /// <summary>
        /// Handles changes to the Event property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CommandBinding)d).OnEventChanged(e);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.isDisposed)
            {
                // If disposing managed and unmanaged resources.
                if (disposing)
                {
                    if (this.behavior != null)
                    {
                        this.behavior.Dispose();
                    }
                }

                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Resets the event binding.
        /// </summary>
        private void ResetEventBinding()
        { 
            // Only do this when the Owner is set.
            if (this.Owner != null)
            {
                // Check if the Event is set. If yes we need to rebind the Command to the new event and unregister the old one.
                if ((this.Behavior.Event != null) && (this.Behavior.Owner != null))
                {
                    this.Behavior.Dispose();
                }

                // Bind the new event to the command.
                this.Behavior.BindEvent(this.Owner, this.Event);
            }
        } 

        #endregion
    }
}
