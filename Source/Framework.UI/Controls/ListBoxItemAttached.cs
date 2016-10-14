namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// <see cref="ListBoxItem"/> attached properties.
    /// </summary>
    public static class ListBoxItemAttached
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsDeselectionEnabledProperty = DependencyProperty.RegisterAttached(
            "IsDeselectionEnabled",
            typeof(bool),
            typeof(ListBoxItemAttached),
            new PropertyMetadata(false, OnIsDeselectionEnabledChanged));

        public static readonly DependencyProperty MoveAboveCommandProperty = DependencyProperty.RegisterAttached(
            "MoveAboveCommand",
            typeof(ICommand),
            typeof(ListBoxItemAttached),
            new PropertyMetadata(null));

        public static readonly DependencyProperty MoveBelowCommandProperty = DependencyProperty.RegisterAttached(
            "MoveBelowCommand",
            typeof(ICommand),
            typeof(ListBoxItemAttached),
            new PropertyMetadata(null));

        public static readonly DependencyProperty MoveDragContentTemplateProperty = DependencyProperty.RegisterAttached(
            "MoveDragContentTemplate", 
            typeof(DataTemplate),
            typeof(ListBoxItemAttached), 
            new PropertyMetadata(null));

        public static readonly DependencyProperty MoveDragFormatProperty = DependencyProperty.RegisterAttached(
            "MoveDragFormat", 
            typeof(string),
            typeof(ListBoxItemAttached), 
            new PropertyMetadata(null));

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the deselection enabled property. If enabled, and the row is clicked while selected, the row is deselected.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <returns>
        ///   <c>true</c> if deselecting row when selected and clicked, otherwise <c>false</c>.
        /// </returns>
        public static bool GetIsDeselectionEnabled(ListBoxItem listBoxItem)
        {
            return (bool)listBoxItem.GetValue(IsDeselectionEnabledProperty);
        }

        /// <summary>
        /// Sets the deselection enabled property. If enabled, and the row is clicked while selected, the row is deselected.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <param name="value">if set to <c>true</c> deselects the row when selected and clicked.</param>
        public static void SetIsDeselectionEnabled(ListBoxItem listBoxItem, bool value)
        {
            listBoxItem.SetValue(IsDeselectionEnabledProperty, value);
        }

        /// <summary>
        /// Gets the command used to move another row above this one using drag and drop.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <returns>
        /// The command to move a row above this instance.
        /// </returns>
        public static ICommand GetMoveAboveCommand(ListBoxItem listBoxItem)
        {
            return (ICommand)listBoxItem.GetValue(MoveAboveCommandProperty);
        }

        /// <summary>
        /// Sets the command used to move another row above this one using drag and drop.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <param name="command">The command to move a row above this instance.</param>
        public static void SetMoveAboveCommand(ListBoxItem listBoxItem, ICommand command)
        {
            listBoxItem.SetValue(MoveAboveCommandProperty, command);
        }

        /// <summary>
        /// Gets the command used to move another row below this one using drag and drop.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <returns>
        /// The command to move a row below this instance.
        /// </returns>
        public static ICommand GetMoveBelowCommand(ListBoxItem listBoxItem)
        {
            return (ICommand)listBoxItem.GetValue(MoveBelowCommandProperty);
        }

        /// <summary>
        /// Sets the command used to move another row below this one using drag and drop.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <param name="command">The command to move a row below this instance.</param>
        public static void SetMoveBelowCommand(ListBoxItem listBoxItem, ICommand command)
        {
            listBoxItem.SetValue(MoveBelowCommandProperty, command);
        }

        /// <summary>
        /// Gets the content template when this instance is being dragged above or below another row.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <returns>
        /// A data template used when this instance is being dragged above or below another row.
        /// </returns>
        public static DataTemplate GetMoveDragContentTemplate(ListBoxItem listBoxItem)
        {
            return (DataTemplate)listBoxItem.GetValue(MoveDragContentTemplateProperty);
        }

        /// <summary>
        /// Sets the content template when this instance is being dragged above or below another row.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <param name="value">A data template used when this instance is being dragged above or below another row.</param>
        public static void SetMoveDragContentTemplate(ListBoxItem listBoxItem, DataTemplate value)
        {
            listBoxItem.SetValue(MoveDragContentTemplateProperty, value);
        }

        /// <summary>
        /// Gets the drag format when this instance is being dragged above or below another row.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <returns>
        /// The drag format used when this instance is being dragged above or below another row.
        /// </returns>
        public static string GetMoveDragFormat(ListBoxItem listBoxItem)
        {
            return (string)listBoxItem.GetValue(MoveDragFormatProperty);
        }

        /// <summary>
        /// Sets the drag format when this instance is being dragged above or below another row.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <param name="format">The drag format used when this instance is being dragged above or below another row.</param>
        public static void SetMoveDragFormat(ListBoxItem listBoxItem, string format)
        {
            listBoxItem.SetValue(MoveDragFormatProperty, format);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the deselection enabled property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsDeselectionEnabledChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)dependencyObject;
            if (GetIsDeselectionEnabled(listBoxItem))
            {
                listBoxItem.PreviewMouseLeftButtonDown += OnListBoxItemMouseLeftButtonDown;
            }
            else
            {
                listBoxItem.PreviewMouseLeftButtonDown -= OnListBoxItemMouseLeftButtonDown;
            }
        }

        /// <summary>
        /// Called when the list box item mouse left button is down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private static void OnListBoxItemMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem listBoxItem = sender as ListBoxItem;
            if (listBoxItem.IsSelected)
            {
                listBoxItem.IsSelected = false;
                e.Handled = true;
            }
        }

        #endregion
    }
}
