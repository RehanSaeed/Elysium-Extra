namespace Framework.UI
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Defines a behaviour that executes a <see cref="ICommand"/> when the specified format of data
    /// is dropped onto the control.
    /// <example>
    /// <![CDATA[
    /// <TextBox Framework:DragCommand.Command="{Binding AddCustomerCommand}"
    ///          Framework:DragCommand.Format="Customer"
    ///          Text="Type Enter to execute command"/>
    /// ]]>
    /// </example>
    /// </summary>
    public sealed class DragCommand
    {
        #region Attached Properties

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(DragCommand),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty FormatProperty = DependencyProperty.RegisterAttached(
            "Format",
            typeof(string),
            typeof(DragCommand),
            new UIPropertyMetadata(null, new PropertyChangedCallback(OnFormatChanged)));

        private static readonly DependencyPropertyKey IsDragOverPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsDragOver", 
            typeof(bool), 
            typeof(DragCommand), 
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsDragOverProperty = IsDragOverPropertyKey.DependencyProperty;

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
        /// Gets the format pressed to invoke the command.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The format of the dragged item.</returns>
        public static string GetFormat(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return (string)dependencyObject.GetValue(FormatProperty);
        }

        /// <summary>
        /// Gets whether the specified object is being dragged over.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns><c>true</c> if the mouse is currently dragging an object over the specified object, otherwise<c>false</c>.</returns>
        public static bool GetIsDragOver(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return (bool)dependencyObject.GetValue(IsDragOverProperty);
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
        /// Sets the format of the dragged item.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetFormat(DependencyObject dependencyObject, string value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(FormatProperty, value);
        }

        /// <summary>
        /// Sets whether the specified object is being dragged over.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">If set to <c>true</c> the mouse is currently dragging an object over the specified object.</param>
        private static void SetIsDragOver(DependencyObject dependencyObject, bool value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(IsDragOverPropertyKey, value);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when the format is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFormatChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            UIElement uiElement = dependencyObject as UIElement;

            if (uiElement != null)
            {
                uiElement.DragEnter -= OnDragEnter;
                uiElement.DragLeave -= OnDragLeave;
                uiElement.DragOver -= OnDragOver;
                uiElement.Drop -= OnDrop;

                uiElement.DragEnter += OnDragEnter;
                uiElement.DragLeave += OnDragLeave;
                uiElement.DragOver += OnDragOver;
                uiElement.Drop += OnDrop;
            }
        }

        /// <summary>
        /// Called when an item is dragged over the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private static void OnDragEnter(object sender, DragEventArgs e)
        {
            UIElement uiElement = (UIElement)sender;
            string format = GetFormat(uiElement);

            if (!e.Data.GetDataPresent(format))
            {
                e.Effects = DragDropEffects.None;
                SetIsDragOver(uiElement, false);
            }
            else
            {
                object data = e.Data.GetData(format);
                ICommand command = GetCommand(uiElement);

                if ((command == null) || !command.CanExecute(data))
                {
                    e.Effects = DragDropEffects.None;
                    SetIsDragOver(uiElement, false);
                }
                else
                {
                    SetIsDragOver(uiElement, true);
                }
            }

            e.Handled = true;
        }

        /// <summary>
        /// Called when an item is dragged away from the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private static void OnDragLeave(object sender, DragEventArgs e)
        {
            OnDragEnter(sender, e);

            UIElement uiElement = (UIElement)sender;
            SetIsDragOver(uiElement, false);
        }

        /// <summary>
        /// Called when an item is dragged over the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private static void OnDragOver(object sender, DragEventArgs e)
        {
            OnDragEnter(sender, e);
        }

        /// <summary>
        /// Called when an item is dropped on to the item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private static void OnDrop(object sender, DragEventArgs e)
        {
            UIElement uiElement = (UIElement)sender;
            string format = GetFormat(uiElement);

            if (e.Data.GetDataPresent(format))
            {
                object data = e.Data.GetData(format);
                ICommand command = GetCommand(uiElement);

                if (command.CanExecute(data))
                {
                    command.Execute(data);
                }
            }

            SetIsDragOver(uiElement, false);
        }

        #endregion
    }
}