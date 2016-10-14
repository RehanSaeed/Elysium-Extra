namespace Framework.UI.Commands
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Defines the attached properties to create a <see cref="CommandBehaviorBinding"/>.
    /// </summary>
    public partial class EventCommand
    {
        #region Dependency Properties

        /// <summary>
        /// Action Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ActionProperty = DependencyProperty.RegisterAttached(
            "Action", 
            typeof(Action<object>), 
            typeof(EventCommand),
            new FrameworkPropertyMetadata(
                (Action<object>)null,
                new PropertyChangedCallback(OnActionChanged)));

        /// <summary>
        /// Command Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", 
            typeof(ICommand), 
            typeof(EventCommand),
            new FrameworkPropertyMetadata(
                (ICommand)null,
                new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// CommandParameter Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter", 
            typeof(object), 
            typeof(EventCommand),
            new FrameworkPropertyMetadata(
                (object)null,
                new PropertyChangedCallback(OnCommandParameterChanged)));

        /// <summary>
        /// Event Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty EventProperty = DependencyProperty.RegisterAttached(
            "Event", 
            typeof(string), 
            typeof(EventCommand),
            new FrameworkPropertyMetadata(
                (string)string.Empty,
                new PropertyChangedCallback(OnEventChanged)));

        /// <summary>
        /// Behaviour Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty BehaviorProperty = DependencyProperty.RegisterAttached(
            "Behavior",
            typeof(CommandBehaviorBinding),
            typeof(EventCommand),
            new FrameworkPropertyMetadata((CommandBehaviorBinding)null));

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the Action property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <returns>The action.</returns>
        public static Action<object> GetAction(DependencyObject d)
        {
            return (Action<object>)d.GetValue(ActionProperty);
        }

        /// <summary>
        /// Sets the Action property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="value">The action value.</param>
        public static void SetAction(DependencyObject d, Action<object> value)
        {
            d.SetValue(ActionProperty, value);
        }

        /// <summary>
        /// Gets the Command property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <returns>The command.</returns>
        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the Command property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="value">The command value.</param>
        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets the CommandParameter property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <returns>The command parameter.</returns>
        public static object GetCommandParameter(DependencyObject d)
        {
            return (object)d.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Sets the CommandParameter property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="value">The command parameter value.</param>
        public static void SetCommandParameter(DependencyObject d, object value)
        {
            d.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Gets the event name property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <returns>The event name.</returns>
        public static string GetEvent(DependencyObject d)
        {
            return (string)d.GetValue(EventProperty);
        }

        /// <summary>
        /// Sets the event name property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="value">The event name.</param>
        public static void SetEvent(DependencyObject d, string value)
        {
            d.SetValue(EventProperty, value);
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
            CommandBehaviorBinding binding = FetchOrCreateBinding(d);
            binding.Action = (Action<object>)e.NewValue;
        }

        /// <summary>
        /// Gets the behaviour property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The command behaviour binding.</returns>
        private static CommandBehaviorBinding GetBehavior(DependencyObject dependencyObject)
        {
            return (CommandBehaviorBinding)dependencyObject.GetValue(BehaviorProperty);
        }

        /// <summary>
        /// Sets the behaviour property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The command behaviour binding.</param>
        private static void SetBehavior(DependencyObject dependencyObject, CommandBehaviorBinding value)
        {
            dependencyObject.SetValue(BehaviorProperty, value);
        }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            CommandBehaviorBinding binding = FetchOrCreateBinding(dependencyObject);
            binding.Command = (ICommand)e.NewValue;
        }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCommandParameterChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            CommandBehaviorBinding binding = FetchOrCreateBinding(dependencyObject);
            binding.CommandParameter = e.NewValue;
        }

        /// <summary>
        /// Handles changes to the Event property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnEventChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            CommandBehaviorBinding binding = FetchOrCreateBinding(dependencyObject);
            
            // Check if the Event is set. If yes we need to rebind the Command to the new event and unregister the old one
            if (binding.Event != null && binding.Owner != null)
            {
                binding.Dispose();
            }

            // Bind the new event to the command
            if (e.NewValue != null && !string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                binding.BindEvent(dependencyObject, e.NewValue.ToString());
            }
        }

        /// <summary>
        /// Tries to get a <see cref="CommandBehaviorBinding"/> from the element. Creates a new instance if there is not one attached.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The command behaviour binding.</returns>
        private static CommandBehaviorBinding FetchOrCreateBinding(DependencyObject dependencyObject)
        {
            CommandBehaviorBinding binding = EventCommand.GetBehavior(dependencyObject);
            
            if (binding == null)
            {
                binding = new CommandBehaviorBinding();
                EventCommand.SetBehavior(dependencyObject, binding);
            }

            return binding;
        }

        #endregion
    }
}
