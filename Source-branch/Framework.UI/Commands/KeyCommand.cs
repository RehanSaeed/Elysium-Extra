namespace Framework.UI.Commands
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Defines a behaviour that executes a <see cref="ICommand"/> when the specified 
    /// <see cref="Key"/> is pressed.
    /// <example>
    /// <![CDATA[
    /// <TextBox Framework:KeyCommand.Key="Return"
    ///          Framework:KeyCommand.Command="{Binding KeyCommand}"
    ///          Framework:KeyCommand.CommandParameter="KeyCommand Parameter"
    ///          Text="Type Enter to execute command"/>
    /// ]]>
    /// </example>
    /// </summary>
    public static class KeyCommand
    {
        #region Attached Properties

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(KeyCommand),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter",
            typeof(object),
            typeof(KeyCommand),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached(
            "Key",
            typeof(Key),
            typeof(KeyCommand),
            new UIPropertyMetadata(Key.None, new PropertyChangedCallback(OnKeyChanged)));

        public static readonly DependencyProperty ModifierProperty = DependencyProperty.RegisterAttached(
            "Modifier",
            typeof(ModifierKeys),
            typeof(KeyCommand),
            new UIPropertyMetadata(ModifierKeys.None, null)); 

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The command.</returns>
        public static ICommand GetCommand(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return (ICommand)dependencyObject.GetValue(CommandProperty);
        }

        /// <summary>
        /// Gets the command parameter.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The command parameter.</returns>
        public static object GetCommandParameter(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return (object)dependencyObject.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Gets the key pressed to invoke the command.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The <see cref="Key"/> pressed to invoke the command.</returns>
        public static Key GetKey(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return (Key)dependencyObject.GetValue(KeyProperty);
        }

        /// <summary>
        /// Gets the modifier pressed to invoke the command.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The <see cref="Key"/> pressed to invoke the command.</returns>
        public static ModifierKeys GetModifier(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return (ModifierKeys)dependencyObject.GetValue(ModifierProperty);
        }

        /// <summary>
        /// Sets the command.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetCommand(DependencyObject dependencyObject, ICommand value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Sets the command parameter.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetCommandParameter(DependencyObject dependencyObject, object value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Sets the key.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetKey(DependencyObject dependencyObject, Key value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(KeyProperty, value);
        }

        /// <summary>
        /// Sets the modifier.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetModifier(DependencyObject dependencyObject, ModifierKeys value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(ModifierProperty, value);
        } 

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Called when the key is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnKeyChanged(
            DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs e)
        {
            UIElement uiElement = dependencyObject as UIElement;

            if (uiElement != null)
            {
                uiElement.KeyDown += new KeyEventHandler(OnKeyDown);
            }
        }

        /// <summary>
        /// Called when the key down event occurs.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            UIElement uiElement = (UIElement)sender;
            
            if (e.Key == GetKey(uiElement) && e.KeyboardDevice.Modifiers == GetModifier(uiElement))
            {
                ICommand command = GetCommand(uiElement);
                object commandParameter = GetCommandParameter(uiElement);

                if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }

        #endregion
    }
}