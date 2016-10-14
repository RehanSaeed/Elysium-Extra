namespace Framework.UI.Commands
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Defines the command behaviour binding
    /// </summary>
    internal class CommandBehaviorBinding : IDisposable
    {
        private Action<object> action;
        private ICommand command;
        private object commandParameter;
        private bool disposed;

        /// <summary>
        /// Stores the strategy of how to execute the event handler.
        /// </summary>
        private IExecutionStrategy strategy;

        #region Public Properties

        /// <summary>
        /// Gets or sets the Action
        /// </summary>
        public Action<object> Action
        {
            get
            {
                return this.action;
            }

            set
            {
                this.action = value;

                // Set the execution strategy to execute the action
                this.strategy = new ActionExecutionStrategy { Behavior = this };
            }
        }

        /// <summary>
        /// Gets or sets the command to execute when the specified event is raised.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return this.command;
            }

            set
            {
                this.command = value;

                // Set the execution strategy to execute the command
                this.strategy = new CommandExecutionStrategy { Behavior = this };
            }
        }

        /// <summary>
        /// Gets or sets a CommandParameter
        /// </summary>
        public object CommandParameter
        {
            get { return this.commandParameter; }
            set { this.commandParameter = value; }
        }

        /// <summary>
        /// Gets the event info of the event
        /// </summary>
        public EventInfo Event { get; private set; }

        /// <summary>
        /// Gets the EventHandler for the binding with the event.
        /// </summary>
        public Delegate EventHandler { get; private set; }

        /// <summary>
        /// Gets the event name to hook up to this property can only be set from the BindEvent Method.
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// Gets the owner of the CommandBinding ex: a Button this property can only be set from the BindEvent Method.
        /// </summary>
        public DependencyObject Owner { get; private set; }
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Creates an EventHandler on runtime and registers that handler to the specified event.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <exception cref="System.InvalidOperationException">Throws if the event name could not be resolved.</exception>
        public void BindEvent(DependencyObject owner, string eventName)
        {
            this.EventName = eventName;
            this.Owner = owner;
            this.Event = this.Owner.GetType().GetEvent(this.EventName, BindingFlags.Public | BindingFlags.Instance);
            if (this.Event == null)
            {
                throw new InvalidOperationException(string.Format("Could not resolve event name {0}", this.EventName));
            }

            // Create an event handler for the event that will call the ExecuteCommand method
            this.EventHandler = EventHandlerGenerator.CreateDelegate(
                this.Event.EventHandlerType, 
                typeof(CommandBehaviorBinding).GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance), 
                this);
            
            // Register the handler to the Event
            this.Event.AddEventHandler(this.Owner, this.EventHandler);
        }

        /// <summary>
        /// Unregisters the eventHandler from the event.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposed)
            {
                this.Event.RemoveEventHandler(this.Owner, this.EventHandler);
                this.disposed = true;
            }
        }

        /// <summary>
        /// Executes the strategy.
        /// </summary>
        public void Execute()
        {
            if (this.strategy == null)
            {
                return;
            }

            this.strategy.Execute(this.CommandParameter);
        } 

        #endregion
    }
}
